using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Police_Crime_Diary.Service
{
    [ServiceContract]
    public interface IFileUploadService
    {
        
        void UploadFile(Stream fileStream);
    }
    
}
