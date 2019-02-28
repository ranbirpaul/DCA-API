using Business_Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
	public interface IDCATypeRepository
	{
		DCATypeModel GetDCAType(string dcaType);
		DCATypeModel GetDCATypeofModelType(string model);
		List<DCATypeModel> GetDCATypes();
		bool CreateDCATypes(DCATypeModel model);
		Task<dynamic> RemoveDCATypes(DCATypeModel model);
		bool UpdateDCATypes(DCATypeModel model);
	}
}
