-- Поиск книг

CREATE OR REPLACE FUNCTION search_books(p_query TEXT, p_search_type INT, p_limit INT, p_offset INT)
RETURNS SETOF books
LANGUAGE sql
AS $$
    WITH q AS (
        SELECT CASE WHEN p_query IS NULL OR p_query = '' THEN NULL
               ELSE '%' || replace(replace(replace(p_query, '\', '\\'), '%', '\%'), '_', '\_') || '%'
               END AS pattern
    )
    SELECT b.*
    FROM books b, q
    WHERE q.pattern IS NULL
       OR CASE p_search_type
            WHEN 0 THEN b.title ILIKE q.pattern
            WHEN 1 THEN b.author ILIKE q.pattern
            WHEN 2 THEN b.description ILIKE q.pattern
            WHEN 3 THEN regexp_replace(b.table_of_contents::text, '<[^>]*>', ' ', 'g') ILIKE q.pattern
            ELSE FALSE
          END
    ORDER BY b.id
    LIMIT p_limit OFFSET p_offset;
$$;

-- Подсчёт книг

CREATE OR REPLACE FUNCTION count_books(p_query TEXT, p_search_type INT)
RETURNS INT
LANGUAGE sql
AS $$
    SELECT COUNT(*)::INT
    FROM search_books(p_query, p_search_type, NULL, 0);
$$;

-- Вставка новой книги

CREATE OR REPLACE PROCEDURE insert_book(
    p_title VARCHAR(255),
    p_description TEXT,
    p_author VARCHAR(255),
    p_release_date DATE,
    p_table_of_contents XML,
    OUT p_id INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO books (title, description, author, release_date, table_of_contents)
    VALUES (p_title, p_description, p_author, p_release_date, p_table_of_contents)
    RETURNING id INTO p_id;
END;
$$;

-- Обновление книги

CREATE OR REPLACE PROCEDURE update_book(
	p_id INT,
    p_title VARCHAR(255),
    p_description TEXT,
    p_author VARCHAR(255),
    p_release_date DATE,
    p_table_of_contents XML,
    OUT p_affected INT)
LANGUAGE plpgsql
AS $$
BEGIN
		UPDATE books
		SET title = p_title,
		description = p_description,
		author = p_author,
		release_date = p_release_date,
		table_of_contents = p_table_of_contents
		WHERE id = p_id;

		GET DIAGNOSTICS p_affected = ROW_COUNT;
END;
$$;

-- Удаление книги

CREATE OR REPLACE PROCEDURE delete_book(p_id INT, OUT p_affected INT)
LANGUAGE plpgsql
AS $$
BEGIN
	DELETE FROM books WHERE id = p_id;
	GET DIAGNOSTICS p_affected = ROW_COUNT;
END;
$$;

-- Получение одной книги

CREATE OR REPLACE FUNCTION get_book_by_id(p_id INT)
RETURNS SETOF books
LANGUAGE sql
AS $$
    SELECT * FROM books WHERE id = p_id;
$$;