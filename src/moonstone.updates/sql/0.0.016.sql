-- 0.0.16

ALTER TABLE [core].[users] ADD tzdbTimeZoneId NVARCHAR(128) NULL;
ALTER TABLE [core].[users] ADD autoUpdateTimeZone BIT NOT NULL;