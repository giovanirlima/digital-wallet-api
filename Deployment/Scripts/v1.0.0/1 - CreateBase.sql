-- Cria o schema caso não exista bank
CREATE SCHEMA IF NOT EXISTS Bank;

-- Cria tabela endereço
CREATE TABLE IF NOT EXISTS Bank."Address" (
    "Id" INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    "Street" VARCHAR(50) NOT NULL,
    "Number" INTEGER NOT NULL,
    "City" VARCHAR(50) NOT NULL,
    "Country" VARCHAR(50) NOT NULL
);

-- Cria tabela usuario
CREATE TABLE IF NOT EXISTS Bank."User" (
    "Id" INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	"WalletId" INTEGER NULL,
    "AddressId" INTEGER NOT NULL,
    "Name" VARCHAR(50) NOT NULL,
    "Password" VARCHAR(100) NOT NULL,
    "Birthday" DATE NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_user_address FOREIGN KEY ("AddressId")
        REFERENCES Bank."Address"("Id") ON DELETE RESTRICT,

    CONSTRAINT uk_user_email UNIQUE ("Email")
);

-- Cria index
CREATE INDEX IF NOT EXISTS idx_user_name ON Bank."User" ("Name");
CREATE INDEX IF NOT EXISTS idx_user_address_id ON Bank."User" ("AddressId");

-- Cria tabela carteira
CREATE TABLE IF NOT EXISTS Bank."Wallet" (
    "Id" INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    "UserId" INTEGER NOT NULL UNIQUE,
    "Amount" DECIMAL(18,2) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_wallet_user FOREIGN KEY ("UserId")
        REFERENCES Bank."User"("Id") ON DELETE RESTRICT
);

-- Cria tabela transações
CREATE TABLE IF NOT EXISTS Bank."Transaction" (
    "Id" INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    "FromUserId" INTEGER NOT NULL,
    "FromWalletId" INTEGER NOT NULL,
    "ToUserId" INTEGER NOT NULL,
    "ToWalletId" INTEGER NOT NULL,
    "TransactionType" VARCHAR(50) NOT NULL,
    "Amount" DECIMAL(18,2) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_transaction_from_user FOREIGN KEY ("FromUserId")
        REFERENCES Bank."User"("Id") ON DELETE RESTRICT,

    CONSTRAINT fk_transaction_to_user FOREIGN KEY ("ToUserId")
        REFERENCES Bank."User"("Id") ON DELETE RESTRICT,

    CONSTRAINT fk_transaction_from_wallet FOREIGN KEY ("FromWalletId")
        REFERENCES Bank."Wallet"("Id") ON DELETE RESTRICT,

    CONSTRAINT fk_transaction_to_wallet FOREIGN KEY ("ToWalletId")
        REFERENCES Bank."Wallet"("Id") ON DELETE RESTRICT
);

-- Cria index
CREATE INDEX IF NOT EXISTS idx_transaction_from_user_id ON Bank."Transaction" ("FromUserId");
CREATE INDEX IF NOT EXISTS idx_transaction_to_user_id ON Bank."Transaction" ("ToUserId");
CREATE INDEX IF NOT EXISTS idx_transaction_created_at ON Bank."Transaction" ("CreatedAt");