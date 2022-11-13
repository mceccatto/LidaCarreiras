using LidaCarreiras.Funcoes;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.Json.Nodes;

namespace LidaCarreiras.Classes
{
    public class Candidato
    {
        private long? Id { get; set; }
        private string? Nome { get; set; }
        private string? NomeSocial { get; set; }
        private string? Genero { get; set; }
        private string? DataNascimento { get; set; }
        private string? Telefone { get; set; }
        private string? Email { get; set; }
        private string? Sobre { get; set; }
        private string? Senha { get; set; }
        private string? DataCadastro { get; set; }
        private bool? Status { get; set; }

        public Candidato(string nome, string nomeSocial, string genero, string dataNascimento, string telefone, string email, string senha, string dataCadastro, bool status)
        {//REGISTRAR CANDIDATO
            this.Nome = nome;
            this.NomeSocial = nomeSocial;
            this.Genero = genero;
            this.DataNascimento = dataNascimento;
            this.Telefone = telefone;
            this.Email = email;
            this.Senha = senha;
            this.DataCadastro = dataCadastro;
            this.Status = status;
        }

        public Candidato(string email, string senha)
        {//LOGIN
            this.Email = email;
            this.Senha = senha;
        }

        public Candidato(string email)
        {//RECUPERAR ACESSO
            this.Email = email;
        }

        public Candidato(long id, string nome, string nomeSocial, string genero, string dataNascimento, string telefone, string sobre, string senha)
        {//EDITAR INFORMACOES CADASTRAIS
            this.Id = id;
            this.Nome = nome;
            this.NomeSocial = nomeSocial;
            this.Genero = genero;
            this.DataNascimento = dataNascimento;
            this.Telefone = telefone;
            this.Sobre = sobre;
            this.Senha = senha;
        }

        public Candidato(long id)
        {
            this.Id = id;
        }

        public static JsonObject RegistrarCandidato(Candidato candidato)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO candidatos (Nome,NomeSocial,Genero,DataNascimento,Telefone,Email,Senha,DataCadastro,Status) VALUES (@Nome,@NomeSocial,@Genero,@DataNascimento,@Telefone,@Email,@Senha,@DataCadastro,@Status)";
                cmd.Parameters.AddWithValue("@Nome", candidato.Nome);
                cmd.Parameters.AddWithValue("@NomeSocial", (candidato.NomeSocial != "") ? candidato.NomeSocial : null);
                cmd.Parameters.AddWithValue("@Genero", candidato.Genero);
                cmd.Parameters.AddWithValue("@DataNascimento", candidato.DataNascimento);
                cmd.Parameters.AddWithValue("@Telefone", candidato.Telefone);
                cmd.Parameters.AddWithValue("@Email", candidato.Email);
                if(candidato.Senha != null)
                {
                    cmd.Parameters.AddWithValue("@Senha", HashSenha.GeraHash(candidato.Senha));
                }
                cmd.Parameters.AddWithValue("@DataCadastro", candidato.DataCadastro);
                cmd.Parameters.AddWithValue("@Status", candidato.Status);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Registro efetuado com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível registrar no momento!");
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                switch (e.Number)
                {
                    case 1062:
                        retornoJson.Add("MENSAGEM", "E-mail já registrado no sistema!");
                        break;
                    default:
                        retornoJson.Add("MENSAGEM", e.Message);
                        break;
                }
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }

        public static JsonObject LoginCandidato(Candidato candidato)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            string senhaTemporaria = SenhaTemp.Gerar();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT Id FROM candidatos WHERE Email = @Email AND Senha = @Senha";
                cmd.Parameters.AddWithValue("@Email", candidato.Email);
                if(candidato.Senha != null)
                {
                    cmd.Parameters.AddWithValue("@Senha", HashSenha.GeraHash(candidato.Senha));
                }
                cmd.Prepare();
                var executaBusca = cmd.ExecuteReader();
                if (executaBusca.HasRows)
                {
                    while (executaBusca.Read())
                    {
                        retornoJson.Add("CODIGO", 0);
                        retornoJson.Add("MENSAGEM", "Usuário autenticado com sucesso!");
                        retornoJson.Add("ID", long.Parse(executaBusca.GetString("Id")));
                    }
                    cmd.Connection.Close();
                }
                else
                {
                    cmd.Connection.Close();
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Usuário não encontrado ou senha inválida!");
                    return retornoJson;
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }

        public static JsonObject RecuperarSenhaCandidato(Candidato candidato)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            string senhaTemporaria = SenhaTemp.Gerar();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "UPDATE candidatos SET Senha = @Senha WHERE Email = @Email";
                cmd.Parameters.AddWithValue("@Email", candidato.Email);
                cmd.Parameters.AddWithValue("@Senha", HashSenha.GeraHash(senhaTemporaria));
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    if (candidato.Email != null)
                    {
                        EnviarEmail dados = new EnviarEmail(candidato.Email, "Recuperação de senha", "Sua nova senha é: " + senhaTemporaria);
                        string statusEnvio = EnviarEmail.Enviar(dados);
                        if (statusEnvio == "sucesso")
                        {
                            retornoJson.Add("CODIGO", 0);
                            retornoJson.Add("MENSAGEM", "Uma nova senha foi enviada para seu e-mail!");
                        }
                        else
                        {
                            retornoJson.Add("CODIGO", 2);
                            retornoJson.Add("MENSAGEM", statusEnvio);
                        }
                    }
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "E-mail não encontrado na base de dados!");
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }

        public static JsonObject EditarCandidato(Candidato candidato)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                string senhaAtual = "";
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT Senha FROM candidatos WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", candidato.Id);
                cmd.Prepare();
                var executaBusca = cmd.ExecuteReader();
                if (executaBusca.HasRows)
                {
                    while (executaBusca.Read())
                    {
                        senhaAtual = executaBusca.GetString("Senha");
                    }
                    cmd.Connection.Close();
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Usuário não encontrado!");
                    return retornoJson;
                }
                conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "UPDATE candidatos SET Nome = @Nome,NomeSocial = @NomeSocial,Genero = @Genero,DataNascimento = @DataNascimento,Telefone = @Telefone,Sobre = @Sobre,Senha = @Senha WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", candidato.Id);
                cmd.Parameters.AddWithValue("@Nome", candidato.Nome);
                cmd.Parameters.AddWithValue("@NomeSocial", (candidato.NomeSocial != "") ? candidato.NomeSocial : null);
                cmd.Parameters.AddWithValue("@Genero", candidato.Genero);
                cmd.Parameters.AddWithValue("@DataNascimento", candidato.DataNascimento);
                cmd.Parameters.AddWithValue("@Telefone", candidato.Telefone);
                cmd.Parameters.AddWithValue("@Sobre", candidato.Sobre);
                if (candidato.Senha != null)
                {
                    cmd.Parameters.AddWithValue("@Senha", HashSenha.GeraHash(candidato.Senha));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Senha", senhaAtual);
                }
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Informações atualizadas com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível atualizar no momento!");
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }

        public static JsonObject ConsultarCandidato(Candidato candidato)
        {
            var culture = new CultureInfo("pt-BR", false);
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM candidatos WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", candidato.Id);
                cmd.Prepare();
                var executaBusca = cmd.ExecuteReader();
                JsonObject candidatoDados = new JsonObject();
                if (executaBusca.HasRows)
                {
                    while (executaBusca.Read())
                    {
                        candidatoDados.Add("Nome", executaBusca.GetString("Nome"));
                        candidatoDados.Add("NomeSocial", (!executaBusca.IsDBNull(executaBusca.GetOrdinal("NomeSocial"))) ? executaBusca.GetString("NomeSocial") : null);
                        candidatoDados.Add("Genero", executaBusca.GetString("Genero"));
                        candidatoDados.Add("DataNascimento", DateTime.ParseExact(executaBusca.GetString("DataNascimento"), "dd/MM/yyyy HH:mm:ss", culture).ToString("yyyy-MM-dd"));
                        candidatoDados.Add("Telefone", executaBusca.GetString("Telefone"));
                        candidatoDados.Add("Email", executaBusca.GetString("Email"));
                        candidatoDados.Add("Sobre", (!executaBusca.IsDBNull(executaBusca.GetOrdinal("Sobre"))) ? executaBusca.GetString("Sobre") : null);
                        candidatoDados.Add("DataCadastro", executaBusca.GetString("DataCadastro"));
                        candidatoDados.Add("Status", Convert.ToBoolean(executaBusca.GetString("Status")));
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Candidato encontrado com sucesso!");
                    retornoJson.Add("CONTEUDO", candidatoDados);
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Nenhum registro foi encontrado!");
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }
    }
}