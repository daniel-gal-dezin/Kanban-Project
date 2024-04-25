BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "userstable" (
	"email"	TEXT,
	"password"	TEXT,
	PRIMARY KEY("email")
);
CREATE TABLE IF NOT EXISTS "boardstable" (
	"boardid"	INTEGER,
	"name"	TEXT,
	"owner"	TEXT,
	"limitbacklog"	INTEGER,
	"limitinprog"	INTEGER,
	"limitdone"	INTEGER,
	"createdate"	TEXT,
	PRIMARY KEY("boardid")
);
CREATE TABLE IF NOT EXISTS "taskstable" (
	"id"	INTEGER,
	"useremail"	TEXT,
	"title"	TEXT,
	"description"	TEXT,
	"creationtime"	TEXT,
	"duedate"	TEXT,
	"colum"	INTEGER,
	"boardid"	INTEGER,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "boardusertable" (
	"email"	TEXT,
	"boardid"	INTEGER,
	"owner"	INTEGER
);
INSERT INTO "userstable" ("email","password") VALUES ('mail@mail.com','Password1');
COMMIT;
