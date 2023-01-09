INTRODUÇÃO:

Primeiramente configure a string do banco de dados Sql Server nos arquivos appSettings.json e appSettings.Testing.json, o formado da string deve ser esse:

Server=ENDERECO_BANCO;DataBase=NOME_BANCO;Trusted_Connection=True;MultipleActiveResultSets=true...

Em relação as strings de conexão dos arquivos appSettings.json e appSettings.Testing.json, o ideal é que o nome dos bancos nos sejam diferentes, um para cada arquivo. Pois o enquanto o arquivo appSettings.json reúne as configurações da aplicação para atendimento aos usuários, o arquivo appSettings.Testing.json serve exclusivamente para os testes de integração da aplicação. 

----------------

INICIANDO A APLICAÇÃO: 

A Primeira vez que a aplicação for iniciada, o processo pode levar até 30 segundos por que ela estará preparando o banco de dados, criando o banco de dados, as tabelas e inclusive ela realiza uma inserção de dados iniciais, na tabela regras_dias_atrasos e também na tabela AspNetUsers. As próximas execuções do projeto não levarão o mesmo tempo para incicilização.

----------------

A Aplicação possui um recurso de autentiação de usuários, então para acessar as rotas de cadastro de contas a pagar, é necessário estar logado. Já existe um usuário cadastrado por padrão 
no banco de dados quando a aplicação é iniciadad pela primeira vez, você pode cadastrar o seu, mas se preferir utilziar o usuário padrão, as credenciais são: 

{
    "Email": "teste@gmail.com", 
    "Password": "123456" 
}

Após fazer o login, você receberá um JSON web token, e você precisa envia-lo em todas suas requisições a Api para que seja considerado como usuário autenticado. Então adicione o 
token no cabeçalho Authorization, o valor do token deve ser descrito desta forma: Bearer [seu_token]...

Você pode testar os endpoints utilziando a interface gráfica do Swagger ou qualquer outro cliente http como Postman.
