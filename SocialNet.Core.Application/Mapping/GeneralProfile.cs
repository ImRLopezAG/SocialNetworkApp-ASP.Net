using AutoMapper;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Mapping;

public class GeneralProfile : Profile {
  public GeneralProfile() {
    #region UserProfile
    CreateMap<User, UserVm>()
    .ForMember(vm => vm.Posts, opt => opt.Ignore())
    .ForMember(vm => vm.Comments, opt => opt.Ignore())
    .ForMember(vm => vm.Friends, opt => opt.Ignore())
    .ReverseMap()
    .ForMember(entity => entity.Posts, opt => opt.Ignore())
    .ForMember(entity => entity.Comments, opt => opt.Ignore())
    .ForMember(entity => entity.Friends, opt => opt.Ignore());


    CreateMap<User, SaveUserVm>()
    .ForMember(vm => vm.ImageFile, opt => opt.Ignore())
    .ForMember(vm => vm.ConfirmPassword, opt => opt.Ignore())
    .ForMember(vm => vm.UserExists, opt => opt.Ignore())
    .ReverseMap()
    .ForMember(entity => entity.Posts, opt => opt.Ignore())
    .ForMember(entity => entity.Comments, opt => opt.Ignore())
    .ForMember(entity => entity.Friends, opt => opt.Ignore());

    #endregion

    #region PostProfile
    CreateMap<Post, PostVm>()
    .ForMember(vm => vm.Comments, opt => opt.Ignore())
    .ForMember(vm => vm.Comment, opt => opt.Ignore())
    .ForMember(vm => vm.UserImg, opt => opt.Ignore())
    .ForMember(vm => vm.UserName, opt => opt.Ignore())
    .ReverseMap()
    .ForMember(entity => entity.User, opt => opt.Ignore())
    .ForMember(entity => entity.Comments, opt => opt.Ignore());

    CreateMap<Post, SavePostVm>()
    .ForMember(vm => vm.ImageFile, opt => opt.Ignore())
    .ReverseMap()
    .ForMember(entity => entity.User, opt => opt.Ignore())
    .ForMember(entity => entity.Comments, opt => opt.Ignore());
    #endregion

    #region CommentProfile
    CreateMap<Comment, CommentVm>()
    .ForMember(vm => vm.UserImg, opt => opt.Ignore())
    .ForMember(vm => vm.UserName, opt => opt.Ignore())
    .ReverseMap()
    .ForMember(entity => entity.Post, opt => opt.Ignore())
    .ForMember(entity => entity.User, opt => opt.Ignore());

    CreateMap<Comment, SaveCommentVm>()
    .ReverseMap()
    .ForMember(entity => entity.Post, opt => opt.Ignore())
    .ForMember(entity => entity.User, opt => opt.Ignore());

    #endregion

    #region FriendProfile
    CreateMap<Friend, FriendVm>()
    .ForMember(vm => vm.Posts, opt => opt.Ignore())
    .ForMember(vm => vm.Users, opt => opt.Ignore())
    .ForMember(vm => vm.UserName, opt => opt.Ignore())
    .ForMember(vm => vm.UserImg, opt => opt.Ignore())
    .ReverseMap()
    .ForMember(entity => entity.Sender, opt => opt.Ignore());

    CreateMap<Friend, SaveFriendVm>()
    .ReverseMap()
    .ForMember(entity => entity.Sender, opt => opt.Ignore());

    #endregion

  }
}
