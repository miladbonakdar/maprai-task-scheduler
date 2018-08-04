using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kavenegar;
using Kavenegar.Exceptions;
using MapraiScheduler.Models.DTO;

namespace MapraiScheduler.Notifier.Services
{
    public static class SmsService
    {
        public static void SendSms(string to, string message)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(NotifySetting.SmsStatics.SmsApiKey);
                var result = api.Send(NotifySetting.SmsStatics.SmsFromNumber, to, message);
            }
            catch (Kavenegar.Exceptions.ApiException ex)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                Console.Write("Message : " + ex.Message);
            }
            catch (Kavenegar.Exceptions.HttpException ex)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                Console.Write("Message : " + ex.Message);
            }
        }

        public static void SendSmsRange(List<string> toList, string message)
        {
            KavenegarApi api = new KavenegarApi(NotifySetting.SmsStatics.SmsApiKey);
            foreach (string to in toList)
            {
                try
                {
                    var result = api.Send(NotifySetting.SmsStatics.SmsFromNumber, to, message);
                }
                catch (ApiException ex)
                {
                    // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                    Console.Write("Message : " + ex.Message);
                }
                catch (HttpException ex)
                {
                    // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                    Console.Write("Message : " + ex.Message);
                }
            }
        }

        public static string CreateNotifySmsTemplate(NotifyDTO dto, string SmsTemplate)
        {
            string res = SmsTemplate.Replace(NotifySetting.SmsStatics.DateProp, dto.PersianDateTime)
                .Replace(NotifySetting.SmsStatics.PhoneNameProp, dto.ProjectPhoneDetail)
                .Replace(NotifySetting.SmsStatics.DescriptionProp, dto.ProjectDetail);
            return res;
        }
    }
}