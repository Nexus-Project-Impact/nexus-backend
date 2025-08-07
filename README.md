
# Nexus Travel Packages API

## Visão Geral

A Nexus Travel Packages API é uma aplicação desenvolvida em .NET 9 para gerenciamento de pacotes de viagem, reservas, avaliações e mídias associadas. Ela oferece endpoints RESTful para operações CRUD, busca avançada e controle de disponibilidade dos pacotes, visando atender sistemas de turismo, agências e plataformas de viagens. 

## 🌟 Principais Funcionalidades

**Gestão de Pacotes de Viagem:**  
  - Cadastro, atualização, consulta e exclusão de pacotes.
  - Filtros por destino, faixa de valor e datas.
  - Retorno de URLs de imagens dos pacotes.

**Reservas:**  
  - Registro, consulta e listagem de reservas.
  - Associação de viajantes e controle de status.

**Avaliações e Mídias:**  
  - Cadastro e consulta de avaliações de pacotes.
  - Upload e gerenciamento de mídias (imagens).

## 🔐 Segurança

**Autenticação e Autorização:**  
  - Suporte para autenticação JWT e roles (Admin, User), garantindo acesso restrito a endpoints sensíveis.
  - Validação de dados de entrada e tratamento de erros padronizado.

**Proteção de Dados:**  
  - Uso de HTTPS obrigatório.
  - Validação de arquivos enviados (mídias) e sanitização de inputs.

## 🛠️ Modelo de Organização

Baseado no **Domain-Driven Design (DDD)**, que facilita a organização do projeto com manutenção mais eficiente, divisão clara de projetos, expansão facilitada e uso de arquitetura limpa (clean architecture).


**Camadas:**  
  - **API:** Controllers e endpoints REST.
  - **Application:** Use Cases e regras de negócio.
  - **Domain:** Entidades e interfaces de repositórios.
  - **Infrastructure:** Implementação de repositórios, UnitOfWork e acesso a dados (Entity Framework).
  - **Communication:** DTOS de Request/Response
  - **Exceptions:** Tratamento de erros

**Padrões Utilizados:**  
  - Repository Pattern para abstração do acesso a dados.
  - Unit of Work para controle transacional.
  - AutoMapper para mapeamento entre entidades e DTOs.
  - Dependency Injection para gerenciamento de dependências.





