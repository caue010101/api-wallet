# 💳 Wallet API

API REST desenvolvida em **.NET 8** simulando uma carteira digital com autenticação JWT.

Este projeto foi criado com foco em boas práticas de arquitetura, separação de camadas e segurança de configuração.

---

## 🚀 Tecnologias Utilizadas

* .NET 8
* ASP.NET Core Web API
* PostgreSQL
* Dapper
* JWT Authentication
* User Secrets (desenvolvimento)
* Variáveis de Ambiente (produção)
* Swagger

---

## 📌 Funcionalidades

* Cadastro de usuário
* Autenticação com geração de JWT
* Criação automática de carteira
* Depósito em carteira
* Saque de saldo
* Proteção de rotas com `[Authorize]`

---

## 🏗️ Estrutura do Projeto

O projeto segue separação em camadas:

* Controllers → Responsável pelos endpoints
* Services → Regras de negócio
* Repositories → Acesso ao banco de dados
* DTOs → Objetos de transferência
* Models → Entidades do domínio

---

## 🔐 Configuração de Segurança

A aplicação utiliza:

* **User Secrets** para desenvolvimento local

## 🎯 Objetivo do Projeto

Este projeto foi desenvolvido com foco em:

* Prática de autenticação com JWT
* Organização em camadas
* Segurança de configuração
* Uso de Dapper para acesso a dados
* Simulação de regras financeiras simples

---

## 👨‍💻 Autor

Caue Alves
Desenvolvedor Backend .NET

---

> Projeto desenvolvido para fins de estudo e evolução profissional.
