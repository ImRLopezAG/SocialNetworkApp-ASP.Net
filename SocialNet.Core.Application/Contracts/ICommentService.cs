using SocialNet.Core.Application.Contracts.Core;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Contracts;

public interface ICommentService : IGenericService<CommentVm, SaveCommentVm, Comment> {

}
