using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.i18n
{
    // see: http://stackoverflow.com/questions/16674008/nodatime-conversions-part-2-how-to
    public static class DateTimeExtensions
    {
        public static DateTime LocalToUtc(this DateTime localDateTime, string zoneId)
        {
            if (localDateTime.Kind != DateTimeKind.Unspecified)
            {
                throw new ArgumentException(
                    $"{nameof(localDateTime)} must have {nameof(localDateTime.Kind)} == {DateTimeKind.Unspecified}");
            }

            LocalDateTime local = LocalDateTime.FromDateTime(localDateTime);
            var timezone = DateTimeZoneProviders.Tzdb[zoneId];

            return timezone.AtLeniently(local).ToDateTimeUtc();
        }

        public static DateTime UtcToLocal(this DateTime utcDateTime, string zoneId)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException(
                    $"{nameof(utcDateTime)} must have {nameof(utcDateTime.Kind)} == {DateTimeKind.Utc}");
            }

            Instant instant = Instant.FromDateTimeUtc(utcDateTime);
            var tz = DateTimeZoneProviders.Tzdb[zoneId];
            return instant.InZone(tz).ToDateTimeUnspecified();
        }
    }
}