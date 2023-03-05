using AutoMapper;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.Core;
using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Services;

public class CommentService : GenericService<CommentVm, SaveCommentVm, Comment>, ICommentService {
  private readonly ICommentRepository _commentRepository;
  private readonly IMapper _mapper;

  public CommentService(ICommentRepository commentRepository, IMapper mapper) : base(commentRepository, mapper) {
    _commentRepository = commentRepository;
    _mapper = mapper;
  }
}
