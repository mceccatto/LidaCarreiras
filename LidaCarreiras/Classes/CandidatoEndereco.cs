using MySql.Data.MySqlClient;
using System.Text.Json.Nodes;

namespace LidaCarreiras.Classes
{
    public class CandidatoEndereco
    {
        private long Candidato { get; set; }
        private string? Logradouro { get; set; }
        private string? Numero { get; set; }
        private string? Complemento { get; set; }
        private string? Cep { get; set; }
        private string? Bairro { get; set; }
        private string? Cidade { get; set; }
        private string? Estado { get; set; }
        private string? DataCadastro { get; set; }

        public CandidatoEndereco(long candidato, string logradouro, string numero, string complemento, string cep, string bairro, string cidade, string estado, string dataCadastro)
        {
            this.Candidato = candidato;
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Cep = cep;
            this.Bairro = bairro;
            this.Cidade = cidade;
            this.Estado = estado;
            this.DataCadastro = dataCadastro;
        }

        public CandidatoEndereco(long candidato, string logradouro, string numero, string complemento, string cep, string bairro, string cidade, string estado)
        {
            this.Candidato = candidato;
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Cep = cep;
            this.Bairro = bairro;
            this.Cidade = cidade;
            this.Estado = estado;
        }

        public CandidatoEndereco(long candidato)
        {
            this.Candidato = candidato;
        }

        public static JsonObject RegistrarEndereco(CandidatoEndereco endereco)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO enderecos (Candidato,Logradouro,Numero,Complemento,Cep,Bairro,Cidade,Estado,DataCadastro) VALUES (@Candidato,@Logradouro,@Numero,@Complemento,@Cep,@Bairro,@Cidade,@Estado,@DataCadastro)";
                cmd.Parameters.AddWithValue("@Candidato", endereco.Candidato);
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

        public static JsonObject EditarEndereco(CandidatoEndereco endereco)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "UPDATE enderecos SET Logradouro = @Logradouro,Numero = @Numero,Complemento = @Complemento,Cep = @Cep,Bairro = @Bairro,Cidade = @Cidade,Estado = @Estado WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", endereco.Candidato);
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
            cmd.CommandText = "SELECT * FROM enderecos WHERE Candidato = @Candidato";
            cmd.Parameters.AddWithValue("@Candidato", id);
            cmd.Prepare();
            var executa = cmd.ExecuteReader();
            if (executa.HasRows)
            {
                retorno = true;
            }
            cmd.Connection.Close();
            return retorno;
        }

        public static JsonObject ConsultarEndereco(CandidatoEndereco endereco)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM enderecos WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", endereco.Candidato);
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonObject candidatoEndereco = new JsonObject();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        candidatoEndereco.Add("Logradouro", executa.GetString("Logradouro"));
                        candidatoEndereco.Add("Numero", executa.GetString("Numero"));
                        candidatoEndereco.Add("Complemento", executa.GetString("Complemento"));
                        candidatoEndereco.Add("Cep", executa.GetString("Cep"));
                        candidatoEndereco.Add("Bairro", executa.GetString("Bairro"));
                        candidatoEndereco.Add("Cidade", executa.GetString("Cidade"));
                        candidatoEndereco.Add("Estado", executa.GetString("Estado"));
                        candidatoEndereco.Add("DataCadastro", executa.GetString("DataCadastro"));
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Endereco encontrado com sucesso!");
                    retornoJson.Add("CONTEUDO", candidatoEndereco);
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

        public static JsonObject ExcluirEndereco(CandidatoEndereco endereco)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM enderecos WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", endereco.Candidato);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Endereço excluído com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível excluir no momento!");
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
