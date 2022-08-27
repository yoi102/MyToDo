using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class PCInforViewModel : BindableBase
    {

        public PCInforViewModel()
        {




            Task.Run(() => GetInfor());


        }


        void GetInfor()
        {

            SysContent = "";
            Content = "";

            // Get the WMI class
            ManagementClass osClass =
                new ManagementClass("Win32_OperatingSystem");

            osClass.Options.UseAmendedQualifiers = true;

            // Get the Properties in the class
            PropertyDataCollection properties =
                osClass.Properties;

            var osClassInstances = osClass.GetInstances();

            //SysContent += osClass["processorid"].ToString();//错的，过时了？
            SysContent += "SystemProperties Name: ";

            foreach (var systemProperties in osClass.SystemProperties)
            {
                SysContent += "\r---------------------------------------";


                foreach (ManagementObject c in osClassInstances)
                {
                    SysContent += $"\rc[systemProperties.Name]: {c[systemProperties.Name]}";
                    SysContent += $"\rName: {systemProperties.Name}";
                    SysContent += "\r";
                    SysContent += $"\rType: {systemProperties.Type}";
                    SysContent += "\r";
                    SysContent += $"\rValue: {systemProperties.Value}";
                    SysContent += "\r";
                    SysContent += $"\rOrigin: {systemProperties.Origin}";
                    SysContent += "\r";
                    try
                    {
                        SysContent += $"\rQualifiers: {systemProperties.Qualifiers["Description"].Value}";
                    }
                    catch
                    {

                    }
                    foreach (QualifierData q in systemProperties.Qualifiers)
                    {
                        SysContent += $"\r{q.Name} : {q.Value}";
                    }

                    SysContent += "\r";

                }


            }







            // display the Property names
            Content += "Property Name: ";
            foreach (PropertyData property in properties)
            {

                Content += "\r---------------------------------------";
                Content += $"\rName: {property.Name}";
                Content += "\r";

                try
                {
                    Content += "\rDescription: " +
                    property.Qualifiers["Description"].Value;
                    Content += "\r";

                }
                catch
                {
                    Content += "\n";
                }

                Content += "\rType: " + property.Type;
                Content += "\r";


                Content += "\rQualifiers: ";


                foreach (QualifierData q in property.Qualifiers)
                {
                    Content += $"\r{q.Name} : {q.Value}";
                }
                Content += "\r";
                foreach (ManagementObject c in osClassInstances)
                {

                    Content += $"\rValue: {c.Properties[property.Name.ToString()].Value}";
                }
                Content += "\r";

            }
        }








        private string _Content = "";

        public string Content
        {
            get { return _Content; }
            set { _Content = value; RaisePropertyChanged(); }
        }

        private string _SysContent = "";

        public string SysContent
        {
            get { return _SysContent; }
            set { _SysContent = value; RaisePropertyChanged(); }
        }




        public DelegateCommand TestCommand => new DelegateCommand(() =>
        {


            Task.Run(() => GetInfor());


        });





    }
}
