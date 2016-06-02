-- 0.0.2

CREATE TABLE [core].[users] (
	id uniqueidentifier not null,
	email nvarchar(256) not null,

	PRIMARY KEY(id)
);

ALTER TABLE [core].[users] ADD  CONSTRAINT [DF_users_id]  DEFAULT (newid()) FOR [id]