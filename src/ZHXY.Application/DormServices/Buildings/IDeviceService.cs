using System.Collections.Generic;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public interface IDeviceService: IAppService
    {
        void BindBuilding(string id, string[] buildings);
        void BindBuildings(string deviceNumber, string buildingIds);
        List<Building> GetBindGate(Relevance relevance);
        List<Building> GetBoundBuildings(string id);
        dynamic GetDormOutInInfo(string buildingIds);
        dynamic GetInOutNumInLatestHours(string buildingIds);
        dynamic GetLatestAppVersion(string currentVersion);
        dynamic GetLatestInOutRecord(string buildingIds);
        List<Device> GetList(Pagination pagination, string keyword = "");
        List<Building> GetNotBoundBuildings(string id);
        dynamic GetSignInfo(string buildingIds);
        dynamic GetVisitorList(string buildingIds, int lastNum = 10);
        void SubmitForm(Device entity);
        void UnbindBuilding(string id, string buildingId);
        void UnBindBuildings(string deviceNumber);
    }
}