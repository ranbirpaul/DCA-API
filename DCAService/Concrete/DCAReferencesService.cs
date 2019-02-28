using Business_Model;
using Repository.Abstract;
using Service.Abstract;
using System;
using System.Threading.Tasks;

namespace Service
{
	public class DCAReferencesService : IDCAReferencesService
	{
		IDCAReferencesRepository _dCARepository;
		public DCAReferencesService(IDCAReferencesRepository dCARepository)
		{
			_dCARepository = dCARepository;
		}

		public async Task<Response> DeleteReferences(string referenceId)
		{
			return await _dCARepository.RemoveReferences(referenceId);
		}
		
		public async Task<dynamic> GetReferences( string objectId, string model)
		{
			return await _dCARepository.GetReferences(objectId, model);
		}
		public async Task<dynamic> GetChildObjectReferences(string objectId, string dcaType)
		{
			return await _dCARepository.GetChildObjects(objectId, dcaType);
		}
		public async Task<Response> GetReferences(string referenceId)
		{
			return await _dCARepository.GetReferences(referenceId);
		}
		public async Task<Response> UpdateReferences(string referenceId, dynamic objects)
		{
			return await _dCARepository.UpdateReferences(referenceId, objects);
		}

		public async Task<Response> PostReferences(string parentObjectId, string parentdcaType,dynamic model)
		{
			return await _dCARepository.SendReferences(parentObjectId, parentdcaType, model);
		}
	}
}
