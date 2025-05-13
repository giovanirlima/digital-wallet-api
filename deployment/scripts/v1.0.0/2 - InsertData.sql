-- 1. Inserir 10 Endereços
WITH address_data AS (
    INSERT INTO Bank."Address" ("Street", "Number", "City", "Country")
    SELECT 'Rua ' || i, 100 + i, 'Cidade ' || i, 'País ' || i
    FROM generate_series(1, 10) AS s(i)
    RETURNING "Id"
),
-- 2. Inserir 10 Carteiras para os 10 usuários
wallet_data AS (
    INSERT INTO Bank."Wallet" ("UserId", "Amount")
    SELECT i, (i * 100.00)::DECIMAL
    FROM generate_series(1, 10) AS s(i)
    RETURNING "Id", "UserId"
),
-- 3. Inserir 10 Usuários
user_data AS (
    INSERT INTO Bank."User" ("AddressId", "Name", "Password", "Birthday", "Email", "WalletId")
    SELECT a."Id", 'Usuário ' || a."Id", '9j0JrsEK0V3PFfLqFgXa9qKX+ABBWg9P6jMASPPt9oCWQjyBOZZsfcO8+azoFuqz', DATE '1990-01-01' + (a."Id" * INTERVAL '365 days'),
           'user' || a."Id" || '@teste.com', w."Id"
    FROM address_data a
    JOIN wallet_data w ON w."UserId" = a."Id"
    RETURNING "Id", "AddressId", "WalletId"
)
-- 4. Inserir 10 Transações entre os usuários
INSERT INTO Bank."Transaction" (
    "FromUserId", "FromWalletId", "ToUserId", "ToWalletId", "TransactionType", "Amount"
)
SELECT 
    fw."UserId", fw."Id",
    tw."UserId", tw."Id",
    'Transfer',
    (fw."UserId" * 10.50)::DECIMAL
FROM wallet_data fw
JOIN wallet_data tw ON fw."UserId" <> tw."UserId"
WHERE fw."UserId" <= 5 AND tw."UserId" > 5
LIMIT 10;