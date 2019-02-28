using Business_Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstract
{
	public interface IDCATypeService
	{
		DCATypeModel GetDCAType(string dcaType);
		List<DCATypeModel> GetDCATypes();
		bool SendDCATypes(DCATypeModel model);
		Task<dynamic> RemoveDCATypes(DCATypeModel model);
		bool UpdateDCATypes(DCATypeModel model);
	}
}
