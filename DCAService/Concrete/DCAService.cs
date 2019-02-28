using Business_Model;
using Repository.Abstract;
using Service.Abstract;
using System;
using System.Threading.Tasks;

namespace Service
{
	public class DCAService : IDCAService
	{
		IDCARepository _dCARepository;
		public DCAService(IDCARepository dCARepository)
		{
			_dCARepository = dCARepository;
		}

		public async Task<Response> DeleteObjects(string objectId, string dcaType)
		{
			return await _dCARepository.RemoveObjects(objectId, dcaType);
		}
		
		public async Task<Response> GetObjects( string objectId, string dcaType)
		{
			return await _dCARepository.GetObjects(objectId, dcaType);
		}
		public async Task<Response> FilterName(string name, string dcaType)
		{
			return await _dCARepository.FilterName(name, dcaType);
		}
		public async Task<Response> UpdateObjects(string objectId, string dcaType, dynamic payload)
		{
			return await _dCARepository.UpdateObjects(objectId, dcaType, payload);
		}

		public async Task<Response> PostObjects(dynamic payload)
		{
			return await _dCARepository.SendObjects(payload, true,true);
		}
		public async Task<Response> PostObjectsWithoutSearch(dynamic payload)
		{
			return await _dCARepository.SendObjects(payload, true, false);
		}
	}
}
