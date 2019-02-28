using Business_Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repository.Abstract;
using Repository.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class DCATypeRepositry : IDCATypeRepository
	{
		string fileName = "DCATypes.json";

		public DCATypeRepositry()
		{

		}

		public DCATypeModel GetDCAType(string dcaType)
		{
			var text = File.ReadAllText(fileName);
			var dcaTypes = JsonConvert.DeserializeObject<List<DCATypeModel>>(text);
			return dcaTypes.Where(o => o.DCAType == dcaType).FirstOrDefault();
		}
		public DCATypeModel GetDCATypeofModelType(string model)
		{
			var text = File.ReadAllText(fileName);
			var dcaTypes = JsonConvert.DeserializeObject<List<DCATypeModel>>(text);
			return dcaTypes.Where(o => o.AbilityModel == model).FirstOrDefault();
		}
		public List<DCATypeModel> GetDCATypes()
		{
			string text = string.Empty;
			text = ReadFile();
			var dcaTypes = JsonConvert.DeserializeObject<List<DCATypeModel>>(text);
			return dcaTypes;
		}
		private string ReadFile()
		{
			string text = string.Empty;
			var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
			{
				text = streamReader.ReadToEnd();
			}
		//	fileStream.Close();
			fileStream.Dispose();

			return text;
		}

		public Task<dynamic> RemoveDCATypes(DCATypeModel model)
		{
			throw new NotImplementedException();
		}
		public bool UpdateDCATypes(DCATypeModel dCATypeModel)
		{
			var dcaTypes = GetDCATypes();
			var newdcaTypes = dcaTypes;
			var objectCount= dcaTypes.Where(o => o.DCAType == dCATypeModel.DCAType).Count();
			if (objectCount > 0)
			{
				var newType = dcaTypes.Where(o => o.DCAType == dCATypeModel.DCAType).FirstOrDefault();
				dcaTypes.Remove(newType);
				newType.AbilityModel = dCATypeModel.AbilityModel;
				newType.AbilityType = dCATypeModel.AbilityType;
				newdcaTypes.Add(newType);
				
				File.Delete(fileName);
				CreateFile(newdcaTypes);
			}
			return true;
		}
		public bool CreateDCATypes(DCATypeModel dCAType)
		{
			if (!File.Exists(fileName))
			{
				List<DCATypeModel> dcaTypes = new List<DCATypeModel>();
				dcaTypes.Add(dCAType);
				CreateFile(dcaTypes);
			}
			else
			{
				var dcaTypes = GetDCATypes();
				if (dcaTypes.Any(o => o.DCAType == dCAType.DCAType))
				{
					return false;
				}
				else
				{
					File.Delete(fileName);
					dcaTypes.Add(dCAType);
					CreateFile(dcaTypes);
				}
			}
			return true;
		}
		private void CreateFile(List<DCATypeModel> dCATypes)
		{
			var json = JsonConvert.SerializeObject(dCATypes);
			var writer = File.CreateText(fileName);
			writer.Write(json);
			writer.Dispose();
		}

		//public async Task<dynamic> SendToAPI(dynamic model)
		//{
		//	var result = await client.PostAsync(editPath, model);
		//	return result;
		//}
	}
}
