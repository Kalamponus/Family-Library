CREATE TABLE IF NOT EXISTS books (
    id                SERIAL PRIMARY KEY,
    title             VARCHAR(255) NOT NULL,
    description       TEXT,
    author            VARCHAR(255) NOT NULL,
    release_date      DATE NOT NULL,
    table_of_contents XML
);
