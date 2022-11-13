using MySql.Data.MySqlClient;
using System.Text.Json.Nodes;

namespace LidaCarreiras.Classes
{
    public class EmpresaEndereco
    {
        private long Empresa { get; set; }
        private string? Logradouro { get; set; }
        private string? Numero { get; set; }
        private string? Complemento { get; set; }
        private string? Cep { get; set; }
        private string? Bairro { get; set; }
        private string? Cidade { get; set; }
        private string? Estado { get; set; }
        private string? DataCadastro { get; set; }

        public EmpresaEndereco(long empresa, string logradouro, string numero, string complemento, string cep, string bairro, string cidade, string estado, string dataCadastro)
        {
            this.Empresa = empresa;
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Cep = cep;
            this.Bairro = bairro;
            this.Cidade = cidade;
            this.Estado = estado;
            this.DataCadastro = dataCadastro;
        }

        public EmpresaEndereco(long empresa, string logradouro, string numero, string complemento, string cep, string bairro, string cidade, string estado)
        {
            this.Empresa = empresa;
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Cep = cep;
            this.Bairro = bairro;
            this.Cidade = cidade;
            this.Estado = estado;
        }

        public EmpresaEndereco(long empresa)
        {
            this.Empresa = empresa;
        }

        public static JsonObject RegistrarEndereco(EmpresaEndereco endereco)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO enderecos (Empresa,Logradouro,Numero,Complemento,Cep,Bairro,Cidade,Estado,DataCadastro) VALUES (@Empresa,@Logradouro,@Numero,@Complemento,@Cep,@Bairro,@Cidade,@Estado,@DataCadastro)";
                cmd.Parameters.AddWithValue("@Empresa", endereco.Empresa);
                cmd.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                cmd.Parameters.AddWithValue("@Numero", (endereco.Numero != null) ? endereco.Numero : null);
                cmd.Parameters.AddWithValue("@Complemento", (endereco.Complemento != null) ? endereco.Complemento : null);
                cmd.Parameters.AddWithValue("@Cep", endereco.Cep);
                cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                cmd.Parameters.AddWithValue("@Estado", endereco.Estado);
                cmd.Parameters.AddWithValue("@DataCadastro", endereco.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Endereco registrado com sucesso!");
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

        public static JsonObject EditarEndereco(EmpresaEndereco endereco)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "UPDATE enderecos SET Logradouro = @Logradouro,Numero = @Numero,Complemento = @Complemento,Cep = @Cep,Bairro = @Bairro,Cidade = @Cidade,Estado = @Estado WHERE Empresa = @Empresa";
                cmd.Parameters.AddWithValue("@Empresa", endereco.Empresa);
                cmd.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                cmd.Parameters.AddWithValue("@Numero", (endereco.Numero != null) ? endereco.Numero : null);
                cmd.Parameters.AddWithValue("@Complemento", (endereco.Complemento != null) ? endereco.Complemento : null);
                cmd.Parameters.AddWithValue("@Cep", endereco.Cep);
                cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                cmd.Parameters.AddWithValue("@Estado", endereco.Estado);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Endereco alterado com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível alterar no momento!");
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

        public static bool ConsultarCadastroEndereco(long id)
        {
            bool retorno = false;
            string conexaoString = StringConexao.Conexao();
            MySqlConnection conexao = new MySqlConnection(conexaoString);
            conexao.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexao;
            cmd.CommandText = "SELECT * FROM enderecos WHERE Empresa = @Empresa";
            cmd.Parameters.AddWithValue("@Empresa", id);
            cmd.Prepare();
            var executa = cmd.ExecuteReader();
            if (executa.HasRows)
            {
                retorno = true;
            }
            cmd.Connection.Close();
            return retorno;
        }

        public static JsonObject ExcluirEndereco(EmpresaEndereco endereco)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM enderecos WHERE Empresa = @Empresa";
                cmd.Parameters.AddWithValue("@Empresa", endereco.Empresa);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Endereço deletado com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível deletar no momento!");
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

        public static JsonObject ConsultarEndereco(EmpresaEndereco endereco)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM enderecos WHERE Empresa = @Empresa";
                cmd.Parameters.AddWithValue("@Empresa", endereco.Empresa);
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonObject empresaEndereco = new JsonObject();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        empresaEndereco.Add("Logradouro", executa.GetString("Logradouro"));
                        empresaEndereco.Add("Numero", executa.GetString("Numero"));
                        empresaEndereco.Add("Complemento", (!executa.IsDBNull(executa.GetOrdinal("Complemento"))) ? executa.GetString("Complemento") : "");
                        empresaEndereco.Add("Cep", executa.GetString("Cep"));
                        empresaEndereco.Add("Bairro", executa.GetString("Bairro"));
                        empresaEndereco.Add("Cidade", executa.GetString("Cidade"));
                        empresaEndereco.Add("Estado", executa.GetString("Estado"));
                        empresaEndereco.Add("DataCadastro", executa.GetString("DataCadastro"));
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Endereco encontrado com sucesso!");
                    retornoJson.Add("CONTEUDO", empresaEndereco);
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
