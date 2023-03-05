using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.Core;
using SocialNet.Core.Application.Helpers;
using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Services;

public class PostService : GenericService<PostVm, SavePostVm, Post>, IPostService {
  private readonly IPostRepository _postRepository;
  private readonly IMapper _mapper;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ICommentRepository _commentRepository;
  private readonly IUserRepository _userRepository;
  private readonly UserVm _currentUser;

  public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICommentRepository commentRepository, IUserRepository userRepository) : base(postRepository, mapper) {
    _postRepository = postRepository;
    _mapper = mapper;
    _httpContextAccessor = httpContextAccessor;
    _commentRepository = commentRepository;
    _userRepository = userRepository;
    _currentUser = _httpContextAccessor.HttpContext.Session.Get<UserVm>("user");
  }
  public override async Task<ServiceResult> GetAll() {
    ServiceResult result = new();
    try {
      var posts = await _postRepository.GetAll().ContinueWith(t => t.Result.Where(p => p.UserId == _currentUser.Id).ToList());
      var comments = await _commentRepository.GetAll();
      var users = await _userRepository.GetAll();

      var query = from post in posts
                  join user in users on post.UserId equals user.Id
                  select _mapper.Map<PostVm>(post, opt => opt.AfterMap((src, pst) => {
                    pst.UserImg = _mapper.Map<UserVm>(user).Image;
                    pst.UserName = _mapper.Map<UserVm>(user).UserName;
                    pst.Comments = comments.Where(c => c.PostId == post.Id).Select(c => _mapper.Map<CommentVm>(c, opt => opt.AfterMap((src, cmt) => {
                      cmt.UserImg = _mapper.Map<UserVm>(users.FirstOrDefault(u => u.Id == c.UserId)).Image;
                      cmt.UserName = _mapper.Map<UserVm>(users.FirstOrDefault(u => u.Id == c.UserId)).UserName;
                    }))).ToList();
                  }));

      result.Data = query.ToList();
    } catch {
      result.Success = false;
      result.Message = "Error while getting posts";
    }
    return result;
  }

}
