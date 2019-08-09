BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Numeracion" (
	"id"	integer NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "Buildings" (
	"rutabuild"	varchar NOT NULL,
	"nameImage"	varchar,
	PRIMARY KEY("rutabuild")
);
CREATE TABLE IF NOT EXISTS "Textura" (
	"Id"	integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	"Nombre"	varchar,
	"Ancho"	integer,
	"Alto"	integer,
	"Image"	blob,
	"Nmipmap"	integer
);
COMMIT;
