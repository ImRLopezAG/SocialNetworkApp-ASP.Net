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

public class FriendService : GenericService<FriendVm, SaveFriendVm, Friend>, IFriendService {
  private readonly IFriendRepository _friendRepository;
  private readonly IPostRepository _postRepository;
  private readonly IUserRepository _userRepository;
  private readonly ICommentRepository _commentRepository;
  private readonly IMapper _mapper;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly UserVm _currentUser;

  public FriendService(IFriendRepository friendRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository) : base(friendRepository, mapper) {
    _friendRepository = friendRepository;
    _mapper = mapper;
    _httpContextAccessor = httpContextAccessor;
    _postRepository = postRepository;
    _userRepository = userRepository;
    _commentRepository = commentRepository;
    _currentUser = _httpContextAccessor.HttpContext.Session.Get<UserVm>("user");
  }
  public async Task SendRequest(int friendId) {
    try {
      var newFriend = new SaveFriendVm {
        SenderId = _currentUser.Id,
        ReceptorId = friendId,
      };
      await _friendRepository.Save(_mapper.Map<Friend>(newFriend));
    } catch (Exception e) {
      throw new Exception(e.Message);
    }
  }

  public override async Task Delete(int id) {
    try {
      var friend = await _friendRepository.GetAll().ContinueWith(t => t.Result.Where(f => f.SenderId == id || f.ReceptorId == id).FirstOrDefault());
      if (friend != null) {
        if (friend.SenderId == _currentUser.Id || friend.ReceptorId == _currentUser.Id) {
          await _friendRepository.Delete(friend);
        }
      }
    } catch (Exception e) { 
      throw new Exception(e.Message);
    }
  }

  public async Task<ServiceResult> GetFriends() {
    ServiceResult result = new();
    try {
      var query = from user in await _userRepository.GetAll()
                  where _friendRepository.AreFriends(_currentUser.Id, user.Id).Result
                  select _mapper.Map<UserVm>(user);
      result.Data = query.ToList();
    } catch (Exception e) {
      throw new Exception(e.Message);
    }
    return result;
  }

  public async Task<ServiceResult> GetFriendsPosts() {
    ServiceResult result = new();
    try {
      var users = await _userRepository.GetAll();
      var myFriends = users.Where(u => _friendRepository.AreFriends(_currentUser.Id, u.Id).Result);
      var posts = await _postRepository.GetAll();
      var comments = await _commentRepository.GetAll();

      var query = from post in posts
                  join user in myFriends on post.UserId equals user.Id
                  select _mapper.Map<PostVm>(post, opt => opt.AfterMap((src, pst) => {
                    pst.UserImg = _mapper.Map<UserVm>(user).Image;
                    pst.UserName = _mapper.Map<UserVm>(user).UserName;
                    pst.Comments = comments.Where(c => c.PostId == post.Id).Select(c => _mapper.Map<CommentVm>(c, opt => opt.AfterMap((src, cmt) => {
                      cmt.UserImg = _mapper.Map<UserVm>(users.FirstOrDefault(u => u.Id == c.UserId)).Image;
                      cmt.UserName = _mapper.Map<UserVm>(users.FirstOrDefault(u => u.Id == c.UserId)).UserName;
                    }))).ToList();
                  }));
      result.Data = query.ToList();
    } catch (Exception e) {
      throw new Exception(e.Message);
    }
    return result;
  }

  public async Task<bool> AreFriends(int friendId) => await _friendRepository.AreFriends(_currentUser.Id, friendId);

}