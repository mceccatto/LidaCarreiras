using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.Json.Nodes;

namespace LidaCarreiras.Classes
{
    public class CandidatoExperiencia
    {
        private long Candidato { get; set; }
        private string? Empresa { get; set; }
        private string? Funcao { get; set; }
        private string? DataContratacao { get; set; }
        private string? DataDesligamento { get; set; }
        private string? Descricao { get; set; }
        private string? DataCadastro { get; set; }

        public CandidatoExperiencia(long candidato, string empresa, string funcao, string dataContratacao, string dataDesligamento, string descricao, string dataCadastro)
        {
            this.Candidato = candidato;
            this.Empresa = empresa;
            this.Funcao = funcao;
            this.DataContratacao = dataContratacao;
            this.DataDesligamento = dataDesligamento;
            this.Descricao = descricao;
            this.DataCadastro = dataCadastro;
        }

        public CandidatoExperiencia(long candidato)
        {
            this.Candidato = candidato;
        }

        public CandidatoExperiencia(long candidato, string dataCadastro)
        {
            this.Candidato = candidato;
            this.DataCadastro = dataCadastro;
        }

        public static JsonObject RegistrarExperiencia(CandidatoExperiencia experiencia)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO candidatos_experiencias (Candidato,Empresa,Funcao,Descricao,DataContratacao,DataDesligamento,DataCadastro) VALUES (@Candidato,@Empresa,@Funcao,@Descricao,@DataContratacao,@DataDesligamento,@DataCadastro)";
                cmd.Parameters.AddWithValue("@Candidato", experiencia.Candidato);
                cmd.Parameters.AddWithValue("@Empresa", experiencia.Empresa);
                cmd.Parameters.AddWithValue("@Funcao", experiencia.Funcao);
                cmd.Parameters.AddWithValue("@Descricao", experiencia.Descricao);
                cmd.Parameters.AddWithValue("@DataContratacao", experiencia.DataContratacao);
                cmd.Parameters.AddWithValue("@DataDesligamento", experiencia.DataDesligamento);
                cmd.Parameters.AddWithValue("@DataCadastro", experiencia.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Experiência registrada com sucesso!");
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

        public static JsonObject EditarExperiencia(CandidatoExperiencia experiencia)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "UPDATE candidatos_experiencias SET Empresa = @Empresa,Funcao = @Funcao,Descricao = @Descricao,DataContratacao = @DataContratacao ,DataDesligamento = @DataDesligamento WHERE Candidato = @Candidato AND DataCadastro = @DataCadastro";
                cmd.Parameters.AddWithValue("@Candidato", experiencia.Candidato);
                cmd.Parameters.AddWithValue("@Empresa", experiencia.Empresa);
                cmd.Parameters.AddWithValue("@Funcao", experiencia.Funcao);
                cmd.Parameters.AddWithValue("@Descricao", experiencia.Descricao);
                cmd.Parameters.AddWithValue("@DataContratacao", experiencia.DataContratacao);
                cmd.Parameters.AddWithValue("@DataDesligamento", experiencia.DataDesligamento);
                cmd.Parameters.AddWithValue("@DataCadastro", experiencia.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Experiência alterada com sucesso!");
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

        public static JsonObject ConsultarExperiencia(CandidatoExperiencia experiencia)
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
                if(experiencia.DataCadastro == "0")
                {
                    cmd.CommandText = "SELECT Empresa, Funcao, Descricao, DataContratacao, DataDesligamento, DATE_FORMAT(DataCadastro, '%Y-%m-%d %H:%i:%s.%f') AS DataCadastro FROM candidatos_experiencias WHERE Candidato = @Candidato";
                    cmd.Parameters.AddWithValue("@Candidato", experiencia.Candidato);
                }
                else
                {
                    cmd.CommandText = "SELECT Empresa, Funcao, Descricao, DataContratacao, DataDesligamento, DATE_FORMAT(DataCadastro, '%Y-%m-%d %H:%i:%s.%f') AS DataCadastro FROM candidatos_experiencias WHERE Candidato = @Candidato AND DataCadastro = @DataCadastro";
                    cmd.Parameters.AddWithValue("@Candidato", experiencia.Candidato);
                    cmd.Parameters.AddWithValue("@DataCadastro", experiencia.DataCadastro);
                }
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonObject experiencias = new JsonObject();
                experiencias.Add("Candidato", experiencia.Candidato);
                JsonArray experienciaArray = new JsonArray();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        JsonObject experienciaTemp = new JsonObject();
                        experienciaTemp.Add("Empresa", executa.GetString("Empresa"));
                        experienciaTemp.Add("Funcao", executa.GetString("Funcao"));
                        experienciaTemp.Add("Descricao", executa.GetString("Descricao"));
                        experienciaTemp.Add("DataContratacao", DateTime.ParseExact(executa.GetString("DataContratacao"), "dd/MM/yyyy HH:mm:ss", culture).ToString("dd/MM/yyyy"));
                        experienciaTemp.Add("DataDesligamento", (!executa.IsDBNull(executa.GetOrdinal("DataDesligamento"))) ? DateTime.ParseExact(executa.GetString("DataDesligamento"), "dd/MM/yyyy HH:mm:ss", culture).ToString("dd/MM/yyyy") : "-");
                        experienciaTemp.Add("DataCadastro", executa.GetString("DataCadastro"));
                        experienciaArray.Add(experienciaTemp);
                    }
                    experiencias.Add("Experiencias", experienciaArray);
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("CONTEUDO", experiencias);
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

        public static JsonObject ExcluirExperiencia(CandidatoExperiencia experiencia)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM candidatos_experiencias WHERE Candidato = @Candidato AND DataCadastro = @DataCadastro";
                cmd.Parameters.AddWithValue("@Candidato", experiencia.Candidato);
                cmd.Parameters.AddWithValue("@DataCadastro", experiencia.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Experiência deletada com sucesso!");
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
    }
}
