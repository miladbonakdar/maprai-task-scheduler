using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MapraiScheduler.Notifier
{
    public static class NotifySetting
    {
        //https://stackoverflow.com/questions/479410/enum-tostring-with-user-friendly-strings
        public enum Priority
        {
            [Description("غیر ضروری")]
            Low = 1,

            [Description("معمولی")]
            Medium = 2,

            [Description("مهم")]
            High = 3,

            [Description("بسیار مهم")]
            SuperHigh = 4,

            [Description("خطرناک و بسیار ضروری")]
            Dangerous = 5
        }

        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        //    "1","زمان انجام عملیت طولانی شده است","زمان انجام عملیات از حد مجاز آن بیشتر شده است . لطفا بررسی شود","2","OutOfTimeProject"
        //    "2","زمان انجام عملیت بسیار طولانی شده است","زمان انجام عملیات بسیار طولانی شده است . در چند ساعت آینده سیستم این عملیات را غیر فعال می کند","3","VeryLateProject"
        //    "3","پروژه متوقف شد","به دلیل طولانی شدن مدت زمان انجام پروژه،متوقف شد.","4","AutoStopProject"
        public static class NotifyTypeUniqueName
        {
            public static string OutOfTimeProject => "OutOfTimeProject";
            public static string VeryLateProject => "VeryLateProject";
            public static string NoUserActivity => "NoUserActivity";
            public static string AutoStopProject => "AutoStopProject";
            public static string LateProjectReport => "LateProjectReport";
            public static string LateDamageReport1 => "LateDamageReport1";
            public static string LateDamageReport2 => "LateDamageReport2";
            public static string LateDamageReport3 => "LateDamageReport3";
        }

        public static class SmsStatics
        {
            public static string SmsApiKey => "434C4A546F345138726B4831337A4538357A643376773D3D";
            public static string SmsFromNumber => "10000066066000";

            public static string PhoneNameProp => "{PhoneName}";
            public static string DescriptionProp => "{Description}";
            public static string DateProp => "{Date}";

            public static string SmsNotifyTemplate => @"رهگیری
نام تعمیر کار:{PhoneName}
رخداد توضیحات :{Description}
تاریخ:{Date}
";

            public static string SmsProblemReportTemplate => @"";
        }

        public static class EmailStatics
        {
            public static string ProjectBaseUrl => @"http://maprai.sepantarai.com/admin/fa/#/ProjectView/";
            public static string PhoneBaseUrl => @"http://maprai.sepantarai.com/admin/fa/#/UserActivity/";
            public static string FromEmailAddress => "raimap.notifier@gmail.com";
            public static string FromEmailPassword => "R@im@p 123456";
            public static string GoogleSmtpAddress => "smtp.gmail.com";
            public static int GoogleSmtpPort => 587;

            public static string EmailTemplate => @"
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<meta http-equiv=""content-type"" content=""text/html; charset=utf-8"">
  	<meta name=""viewport"" content=""width=device-width, initial-scale=1.0;"">
 	<meta name=""format-detection"" content=""telephone=no""/>
	<style>
body { margin: 0; padding: 0; min-width: 100%; width: 100% !important; height: 100% !important;}
body, table, td, div, p, a { -webkit-font-smoothing: antialiased; text-size-adjust: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; line-height: 100%; }
table, td { mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-collapse: collapse !important; border-spacing: 0; }
img { border: 0; line-height: 100%; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; }
#outlook a { padding: 0; }
.ReadMsgBody { width: 100%; } .ExternalClass { width: 100%; }
.ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div { line-height: 100%; }
@media all and (min-width: 560px) {
	.container { border-radius: 8px; -webkit-border-radius: 8px; -moz-border-radius: 8px; -khtml-border-radius: 8px;}
}
a, a:hover {
	color: #127DB3;
}
.footer a, .footer a:hover {
	color: #999999;
}
.persian-rtl {
    direction: rtl;
    white-space: normal;
}
 	</style>
	<title>سیستم اعلام رخداد - رهگیری</title>
</head>
<body topmargin=""0"" rightmargin=""0"" bottommargin=""0"" leftmargin=""0"" marginwidth=""0"" marginheight=""0"" width=""100%"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; width: 100%; height: 100%; -webkit-font-smoothing: antialiased; text-size-adjust: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; line-height: 100%;
	background-color: #F0F0F0;
	color: #000000;""
	bgcolor=""#F0F0F0""
	text=""#000000"">
<table width=""100%"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; width: 100%;"" class=""background""><tr><td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;""
	bgcolor=""#F0F0F0"">
<table border=""0"" cellpadding=""0"" cellspacing=""0"" align=""center""
	bgcolor=""#FFFFFF""
	width=""560"" style=""border-collapse: collapse; border-spacing: 0; padding: 0;
	max-width: 560px;"" class=""container"">
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; font-size: 24px; font-weight: bold; line-height: 130%;
			padding-top: 25px;
			color: #000000;
			font-family: sans-serif;"" class=""header persian-rtl"">
			سیستم اعلام رخداد - رهگیری
		</td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-bottom: 3px; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; font-size: 18px; font-weight: 300; line-height: 150%;
			padding-top: 5px;
			color: #000000;
			font-family: sans-serif;"" class=""subheader persian-rtl"">
			میزان اهمیت - {PriorityName}
		</td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;
			padding-top: 20px;"" class=""hero""><a target=""_blank"" style=""text-decoration: none;""
			href=""http://maprai.sepantarai.com"">
		<div style=""height: 20px;background-color: {NotifyColor}""></div>
		</a></td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%;
			padding-top: 25px;"" class=""line""><hr
			color=""#E0E0E0"" align=""center"" width=""100%"" size=""1"" noshade style=""margin: 0; padding: 0;"" />
		</td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; font-size: 17px; font-weight: 400; line-height: 160%;
			padding-top: 25px;
			color: #000000;
			font-family: sans-serif;"" class=""paragraph persian-rtl"">
			{EventDescription}
		</td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%;
			padding-top: 25px;
			padding-bottom: 5px;"" class=""button""><a
			href=""http://maprai.sepantarai.com"" target=""_blank"" style=""text-decoration: underline;"">
				<table border=""0"" cellpadding=""0"" cellspacing=""0"" align=""center"" style=""max-width: 240px; min-width: 120px; border-collapse: collapse; border-spacing: 0; padding: 0;""><tr><td align=""center"" valign=""middle"" style=""padding: 12px 24px; margin: 0; text-decoration: underline; border-collapse: collapse; border-spacing: 0; border-radius: 4px; -webkit-border-radius: 4px; -moz-border-radius: 4px; -khtml-border-radius: 4px;""
					bgcolor=""#E9703E""><a class=""persian-rtl"" target=""_blank"" style=""text-decoration: underline;
					color: #FFFFFF; font-family: sans-serif; font-size: 17px; font-weight: 400; line-height: 120%;""
					href=""http://maprai.sepantarai.com"">
						ورود به سایت
					</a>
			</td></tr></table></a>
		</td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%;
			padding-top: 25px;"" class=""line""><hr
			color=""#E0E0E0"" align=""center"" width=""100%"" size=""1"" noshade style=""margin: 0; padding: 0;"" />
		</td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%;"" class=""list-item""><table align=""center"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width: inherit; margin: 0; padding: 0; border-collapse: collapse; border-spacing: 0;"">
			<tr>
				<td align=""right"" valign=""right"" style=""border-collapse: collapse; border-spacing: 0;
					padding-top: 30px;
					padding-right: 20px;""><img
				border=""0"" vspace=""0"" hspace=""0"" style=""padding: 0; margin: 0; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; border: none; display: block;
					color: #000000;""
					src=""http://maprai.sepantarai.com/shared/images/EmailTemplate/calendar.png""
					alt=""H"" title=""Highly compatible""
					width=""50"" height=""50""></td>
				<td align=""right"" valign=""right"" style=""font-size: 17px; font-weight: 400; line-height: 160%; border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;
					padding-top: 25px;
					color: #000000;
					font-family: sans-serif;"" class=""paragraph persian-rtl"">
						<b style=""color: #333333;"" class=""persian-rtl"">تاریخ</b><br/>
						{PersianDateTime}
				</td>
			</tr>
			<tr>
				<td align=""right"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0;
					padding-top: 30px;
					padding-right: 20px;""><img
				border=""0"" vspace=""0"" hspace=""0"" style=""padding: 0; margin: 0; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; border: none; display: block;
					color: #000000;""
					src=""http://maprai.sepantarai.com/shared/images/EmailTemplate/innovation.png""
					alt=""D"" title=""Designer friendly""
					width=""50"" height=""50""></td>
				<td align=""right"" valign=""top"" style=""font-size: 17px; font-weight: 400; line-height: 160%; border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;
					padding-top: 25px;
					color: #000000;
					font-family: sans-serif;"" class=""paragraph persian-rtl"">
						<b style=""color: #333333;"" class=""persian-rtl"">پروژه</b><br/>
						{ProjectDetail}
						<br>
						<a style=""text-decoration: none;font-size: 14px"" href=""{ProjectDetailUrl}"">پروژه</a>
					</td>

			</tr>
						<tr>
							<td align=""right"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0;
								padding-top: 30px;
								padding-right: 20px;""><img
							border=""0"" vspace=""0"" hspace=""0"" style=""padding: 0; margin: 0; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; border: none; display: block;
								color: #000000;""
								src=""http://maprai.sepantarai.com/shared/images/EmailTemplate/worker.png""
								alt=""D"" title=""Designer friendly""
								width=""50"" height=""50""></td>
							<td align=""right"" valign=""top"" style=""font-size: 17px; font-weight: 400; line-height: 160%; border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;
								padding-top: 25px;
								color: #000000;
								font-family: sans-serif;"" class=""paragraph persian-rtl"">
									<b style=""color: #333333;"" class=""persian-rtl"">تعمیرکار پروژه</b><br/>
								{ProjectPhoneDetail}
								<br>
								<a style=""text-decoration: none;font-size: 14px"" href=""{ProjectPhoneDetailUrl}"">تعمیرکار</a>
								</td>
						</tr>
			<tr>
				<td align=""right"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0;
					padding-top: 30px;
					padding-right: 20px;""><img
				border=""0"" vspace=""0"" hspace=""0"" style=""padding: 0; margin: 0; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; border: none; display: block;
					color: #000000;""
					src=""http://maprai.sepantarai.com/shared/images/EmailTemplate/employee.png""
					alt=""D"" title=""Designer friendly""
					width=""50"" height=""50""></td>
				<td align=""right"" valign=""top"" style=""font-size: 17px; font-weight: 400; line-height: 160%; border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;
					padding-top: 25px;
					color: #000000;
					font-family: sans-serif;"" class=""paragraph persian-rtl"">
						<b style=""color: #333333;"" class=""persian-rtl"">ادمین پروژه</b><br/>
						{ProjectAdminDetail}
					</td>
			</tr>
		</table></td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%;
			padding-top: 25px;"" class=""line""><hr
			color=""#E0E0E0"" align=""center"" width=""100%"" size=""1"" noshade style=""margin: 0; padding: 0;"" />
		</td>
	</tr>
	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; font-size: 17px; font-weight: 400; line-height: 160%;
			padding-top: 20px;
			padding-bottom: 25px;
			color: #000000;
			font-family: sans-serif;"" class=""paragraph"">
			سوالات خود را از اینجا بپرسید &nbsp;&nbsp;<br> <a href=""mailto:sepanta-domainadmin@sepantarai.com"" target=""_blank"" style=""color: #127DB3; font-family: sans-serif; font-size: 17px; font-weight: 400; line-height: 160%;"">sepanta-domainadmin@sepantarai.com</a>
		</td>
	</tr>
</table>
<table border=""0"" cellpadding=""0"" cellspacing=""0"" align=""center""
	width=""560"" style=""border-collapse: collapse; border-spacing: 0; padding: 0; width: inherit;
	max-width: 560px;"" class=""wrapper"">

	<tr>
		<td align=""center"" valign=""top"" style=""border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; font-size: 13px; font-weight: 400; line-height: 150%;
			padding-top: 20px;
			padding-bottom: 20px;
			color: #999999;
			font-family: sans-serif;"" class=""footer"">
				<img width=""1"" height=""1"" border=""0"" vspace=""0"" hspace=""0"" style=""margin: 0; padding: 0; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; border: none; display: block;""
				src=""https://raw.githubusercontent.com/konsav/email-templates/master/images/tracker.png"" />

		</td>
	</tr>

</table>
</td></tr></table>

</body>
</html>";
        }
    }
}