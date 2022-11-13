using LidaCarreiras.Funcoes;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace LidaCarreiras.Classes
{
    public class Empresa
    {
        private long Id { get; set; }
        private string? Cnpj { get; set; }
        private string? RasaoSocial { get; set; }
        private string? NomeFantasia { get; set; }
        private string? Telefone { get; set; }
        private string? Ramal { get; set; }
        private string? Email { get; set; }
        private string? Descricao { get; set; }
        private string? Logo { get; set; }
        private string? Senha { get; set; }
        private string? DataCadastro { get; set; }
        private bool? Status { get; set; }

        public Empresa(string cnpj, string rasaoSocial, string nomeFantasia, string telefone, string ramal, string email, string senha, string dataCadastro, bool status)
        {
            this.Cnpj = cnpj;
            this.RasaoSocial = rasaoSocial;
            this.NomeFantasia = nomeFantasia;
            this.Telefone = telefone;
            this.Ramal = ramal;
            this.Email = email;
            this.Senha = senha;
            this.DataCadastro = dataCadastro;
            this.Status = status;
        }

        public Empresa(string email, string senha)
        {
            this.Email = email;
            this.Senha = senha;
        }

        public Empresa(long id)
        {
            this.Id = id;
        }

        public Empresa(long id, string rasaoSocial, string nomeFantasia, string telefone, string ramal, string descricao, string logo, string senha)
        {
            this.Id = id;
            this.RasaoSocial = rasaoSocial;
            this.NomeFantasia = nomeFantasia;
            this.Telefone = telefone;
            this.Ramal = ramal;
            this.Descricao = descricao;
            this.Logo = logo;
            this.Senha = senha;
        }

        public static JsonObject RegistrarEmpresa(Empresa empresa)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO empresas (Cnpj,RasaoSocial,NomeFantasia,Telefone,Ramal,Email,Senha,DataCadastro,Status) VALUES (@Cnpj,@RasaoSocial,@NomeFantasia,@Telefone,@Ramal,@Email,@Senha,@DataCadastro,@Status)";
                cmd.Parameters.AddWithValue("@Cnpj", empresa.Cnpj);
                cmd.Parameters.AddWithValue("@RasaoSocial", empresa.RasaoSocial);
                cmd.Parameters.AddWithValue("@NomeFantasia", empresa.NomeFantasia);
                cmd.Parameters.AddWithValue("@Telefone", empresa.Telefone);
                cmd.Parameters.AddWithValue("@Ramal", (empresa.Ramal != null) ? empresa.Ramal : null);
                cmd.Parameters.AddWithValue("@Email", empresa.Email);
                if(empresa.Senha != null)
                {
                    cmd.Parameters.AddWithValue("@Senha", HashSenha.GeraHash(empresa.Senha));
                }
                cmd.Parameters.AddWithValue("@DataCadastro", empresa.DataCadastro);
                cmd.Parameters.AddWithValue("@Status", empresa.Status);
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
                        retornoJson.Add("MENSAGEM", "CNPJ e/ou E-mail já registrado(s) no sistema!");
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

        public static JsonObject LoginEmpresa(Empresa empresa)
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
                cmd.CommandText = "SELECT Id FROM empresas WHERE Email = @Email AND Senha = @Senha";
                cmd.Parameters.AddWithValue("@Email", empresa.Email);
                if (empresa.Senha != null)
                {
                    cmd.Parameters.AddWithValue("@Senha", HashSenha.GeraHash(empresa.Senha));
                }
                cmd.Prepare();
                var executaBusca = cmd.ExecuteReader();
                if (executaBusca.HasRows)
                {
                    while (executaBusca.Read())
                    {
                        retornoJson.Add("CODIGO", 0);
                        retornoJson.Add("MENSAGEM", "Empresa autenticada com sucesso!");
                        retornoJson.Add("ID", long.Parse(executaBusca.GetString("Id")));
                    }
                    cmd.Connection.Close();
                }
                else
                {
                    cmd.Connection.Close();
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Empresa não encontrada ou senha inválida!");
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

        public static JsonObject RecuperarSenhaEmpresa(Empresa empresa)
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
                cmd.CommandText = "UPDATE empresas SET Senha = @Senha WHERE Email = @Email";
                cmd.Parameters.AddWithValue("@Email", empresa.Email);
                cmd.Parameters.AddWithValue("@Senha", HashSenha.GeraHash(senhaTemporaria));
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    if (empresa.Email != null)
                    {
                        EnviarEmail dados = new EnviarEmail(empresa.Email, "Recuperação de senha", "Sua nova senha é: " + senhaTemporaria);
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

        public static JsonObject EditarEmpresa(Empresa empresa)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                string senhaAtual = "", logoAtual = "";
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT Logo, Senha FROM empresas WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", empresa.Id);
                cmd.Prepare();
                var executaBusca = cmd.ExecuteReader();
                if (executaBusca.HasRows)
                {
                    while (executaBusca.Read())
                    {
                        logoAtual = (!executaBusca.IsDBNull(executaBusca.GetOrdinal("Logo"))) ? executaBusca.GetString("Logo") : "";
                        senhaAtual = executaBusca.GetString("Senha");
                    }
                    cmd.Connection.Close();
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Empresa não encontrada!");
                    return retornoJson;
                }
                conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "UPDATE empresas SET RasaoSocial = @RasaoSocial, NomeFantasia = @NomeFantasia, Telefone = @Telefone, Ramal = @Ramal, Descricao = @Descricao, Logo = @Logo, Senha = @Senha WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", empresa.Id);
                cmd.Parameters.AddWithValue("@RasaoSocial", empresa.RasaoSocial);
                cmd.Parameters.AddWithValue("@NomeFantasia", empresa.NomeFantasia);
                cmd.Parameters.AddWithValue("@Telefone", empresa.Telefone);
                cmd.Parameters.AddWithValue("@Ramal", (empresa.Ramal != null) ? empresa.Ramal : null);
                cmd.Parameters.AddWithValue("@Descricao", (empresa.Descricao != null) ? empresa.Descricao : null);
                if (empresa.Logo != null)
                {
                    cmd.Parameters.AddWithValue("@Logo", empresa.Logo);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Logo", (logoAtual != "") ? logoAtual : null);
                }
                if (empresa.Senha != null)
                {
                    cmd.Parameters.AddWithValue("@Senha", HashSenha.GeraHash(empresa.Senha));
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

        public static JsonObject ConsultarEmpresa(Empresa empresa)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM empresas WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", empresa.Id);
                cmd.Prepare();
                var executaBusca = cmd.ExecuteReader();
                JsonObject empresaDados = new JsonObject();
                if (executaBusca.HasRows)
                {
                    while (executaBusca.Read())
                    {
                        empresaDados.Add("Cnpj", executaBusca.GetString("Cnpj"));
                        empresaDados.Add("RasaoSocial", executaBusca.GetString("RasaoSocial"));
                        empresaDados.Add("NomeFantasia", executaBusca.GetString("NomeFantasia"));
                        empresaDados.Add("Telefone", executaBusca.GetString("Telefone"));
                        empresaDados.Add("Ramal", (!executaBusca.IsDBNull(executaBusca.GetOrdinal("Ramal"))) ? executaBusca.GetString("Ramal") : "");
                        empresaDados.Add("Email", executaBusca.GetString("Email"));
                        empresaDados.Add("Descricao", (!executaBusca.IsDBNull(executaBusca.GetOrdinal("Descricao"))) ? executaBusca.GetString("Descricao") : "");
                        empresaDados.Add("Logo", (!executaBusca.IsDBNull(executaBusca.GetOrdinal("Logo"))) ? executaBusca.GetString("Logo") : "");
                        empresaDados.Add("DataCadastro", executaBusca.GetString("DataCadastro"));
                        empresaDados.Add("Status", Convert.ToBoolean(executaBusca.GetString("Status")));
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Empresa encontrada com sucesso!");
                    retornoJson.Add("CONTEUDO", empresaDados);
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