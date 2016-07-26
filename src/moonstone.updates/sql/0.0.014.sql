-- 0.0.14

ALTER TABLE [core].[bankAccounts] DROP CONSTRAINT [FK_bankAccounts_groups_id];
ALTER TABLE [core].[bankAccounts] DROP COLUMN [groupId];