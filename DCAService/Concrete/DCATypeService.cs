using Business_Model;
using Repository.Abstract;
using Service.Abstract;
using System;
using System.Collections.Generic;

namespace Service
{
	public class DCATypeService : IDCATypeService
	{
		IDCATypeRepository _dcaTypeRepository;
		public DCATypeService(IDCATypeRepository dCATypeRepository)
		{
			_dcaTypeRepository = dCATypeRepository;
		}
		public List<DCATypeModel> GetDCATypes()
		{
			return _dcaTypeRepository.GetDCATypes();
		}
		public DCATypeModel GetDCAType(string dcaType)
		{
			return _dcaTypeRepository.GetDCAType(dcaType);
		}
		public System.Threading.Tasks.Task<dynamic> RemoveDCATypes(DCATypeModel model)
		{
			return _dcaTypeRepository.RemoveDCATypes(model);
		}

		public bool SendDCATypes(DCATypeModel model)
		{
			return _dcaTypeRepository.CreateDCATypes(model);
		}
		public bool UpdateDCATypes(DCATypeModel model)
		{
			return _dcaTypeRepository.UpdateDCATypes(model);
		}
	}
}
