DO $$
BEGIN
    -- Verifica se as tabelas existem
    IF EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'Address' AND n.nspname = 'wallet'
    ) AND EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'User' AND n.nspname = 'wallet'
    ) THEN

        INSERT INTO wallet."Address" ("Street", "Number", "City", "Country") VALUES
        ('Rua das Flores', 123, 'São Paulo', 'Brasil'),
        ('Avenida Central', 456, 'Rio de Janeiro', 'Brasil'),
        ('Travessa da Paz', 789, 'Belo Horizonte', 'Brasil'),
        ('Rua do Comércio', 101, 'Curitiba', 'Brasil'),
        ('Avenida Brasil', 202, 'Porto Alegre', 'Brasil'),
        ('Rua das Palmeiras', 303, 'Salvador', 'Brasil'),
        ('Alameda Santos', 404, 'Fortaleza', 'Brasil'),
        ('Rua XV de Novembro', 505, 'Recife', 'Brasil'),
        ('Avenida Paulista', 606, 'São Paulo', 'Brasil'),
        ('Rua do Sol', 707, 'Manaus', 'Brasil');

        INSERT INTO wallet."User" ("AddressId", "Name", "Birthday", "Email") VALUES
        (1, 'Ana Silva', '1990-05-12', 'ana.silva@email.com'),
        (2, 'Bruno Costa', '1988-03-22', 'bruno.costa@email.com'),
        (3, 'Carla Mendes', '1995-11-05', 'carla.mendes@email.com'),
        (4, 'Daniel Rocha', '1982-07-19', 'daniel.rocha@email.com'),
        (5, 'Eduarda Lima', '1993-01-30', 'eduarda.lima@email.com'),
        (6, 'Fernando Alves', '1987-09-14', 'fernando.alves@email.com'),
        (7, 'Gabriela Souza', '1991-06-08', 'gabriela.souza@email.com'),
        (8, 'Henrique Martins', '1996-10-25', 'henrique.martins@email.com'),
        (9, 'Isabela Fernandes', '1989-12-03', 'isabela.fernandes@email.com'),
        (10, 'João Pereira', '1994-08-17', 'joao.pereira@email.com');

    END IF;
END
$$;