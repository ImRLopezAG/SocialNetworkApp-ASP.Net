using SocialNet.Core.Application.Core;
using SocialNet.Core.Domain.Core;

namespace SocialNet.Core.Application.Contracts.Core;
public interface IGenericService<EntityVm, SaveEntityVm, Entity> : IBaseService where EntityVm : BaseVm where SaveEntityVm : class where Entity : BaseEntity {
  Task<SaveEntityVm> Save(SaveEntityVm vm);
  Task Edit(SaveEntityVm vm);
  Task Delete(int id);
  Task<SaveEntityVm> GetEntity(int id);
}
