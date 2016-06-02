-- 0.0.2

CREATE TABLE [core].[users] (
	id UNIQUEIDENTIFIER not null,
	email NVARCHAR(256) not null,

	PRIMARY KEY(id)
);

ALTER TABLE [core].[users] ADD
	CONSTRAINT [DEF_users_id]  DEFAULT (NEWSEQUENTIALID()) FOR [id],
	CONSTRAINT [UN_users_email] UNIQUE (email)