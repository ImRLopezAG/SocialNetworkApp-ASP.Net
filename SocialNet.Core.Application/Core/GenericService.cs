using AutoMapper;
using SocialNet.Core.Application.Contracts.Core;
using SocialNet.Core.Domain.Core;

namespace SocialNet.Core.Application.Core;

public class GenericService<EntityVm, SaveEntityVm, Entity> : IGenericService<EntityVm, SaveEntityVm, Entity> where EntityVm : BaseVm where SaveEntityVm : BaseVm where Entity : BaseEntity {
  private readonly IGenericRepository<Entity> _repository;
  private readonly IMapper _mapper;

  public GenericService(IGenericRepository<Entity> repository, IMapper mapper) {
    _repository = repository;
    _mapper = mapper;
  }

  public virtual async Task<ServiceResult> GetAll() {
    ServiceResult result = new();
    try {
      var query = from entity in await _repository.GetAll()
                  select _mapper.Map<EntityVm>(entity);

      result.Data = query;
    } catch (Exception ex) {
      result.Success = false;
      result.Message = ex.Message;
    }
    return result;
  }

  public virtual async Task<SaveEntityVm> GetEntity(int id) {
    var entity = await _repository.GetEntity(id);
    return _mapper.Map<SaveEntityVm>(entity);
  }

  public virtual async Task<ServiceResult> GetById(int id) {
    ServiceResult result = new();
    try {
      var entity = await _repository.GetEntity(id);
      result.Data = _mapper.Map<EntityVm>(entity);
    } catch (Exception ex) {
      result.Success = false;
      result.Message = ex.Message;
    }
    return result;
  }
  public virtual async Task<SaveEntityVm> Save(SaveEntityVm vm) {
    var entity = _mapper.Map<Entity>(vm);
    await _repository.Save(entity);
    return _mapper.Map<SaveEntityVm>(entity);
  }

  public virtual async Task Edit(SaveEntityVm vm) {
    try {
      if (_repository.GetEntity(vm.Id) != null) {
        var entity = _mapper.Map<Entity>(vm);
        await _repository.Update(entity);
      }

    } catch {
      throw;
    }
  }

  public virtual async Task Delete(int id) {
    var entity = await _repository.GetEntity(id);
    await _repository.Delete(entity);
  }
}

