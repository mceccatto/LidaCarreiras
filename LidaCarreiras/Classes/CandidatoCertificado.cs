using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace LidaCarreiras.Classes
{
    public class CandidatoCertificado
    {
        private long Candidato { get; set; }
        private string? Instituicao { get; set; }
        private string? Titulo { get; set; }
        private string? Anexo { get; set; }
        private string? Url { get; set; }
        private string? DataCadastro { get; set; }

        public CandidatoCertificado(long candidato, string? instituicao, string? titulo, string? anexo, string? url, string dataCadastro)
        {
            this.Candidato = candidato;
            this.Instituicao = instituicao;
            this.Titulo = titulo;
            this.Anexo = anexo;
            this.Url = url;
            this.DataCadastro = dataCadastro;
        }

        public CandidatoCertificado(long candidato)
        {
            this.Candidato = candidato;
        }

        public CandidatoCertificado(long candidato, string dataCadastro)
        {
            this.Candidato = candidato;
            this.DataCadastro = dataCadastro;
        }

        public static JsonObject RegistrarCertificado(CandidatoCertificado certificado)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO candidatos_certificados (Candidato,Instituicao,Titulo,Anexo,Url,DataCadastro) VALUES (@Candidato,@Instituicao,@Titulo,@Anexo,@Url,@DataCadastro)";
                cmd.Parameters.AddWithValue("@Candidato", certificado.Candidato);
                cmd.Parameters.AddWithValue("@Instituicao", certificado.Instituicao);
                cmd.Parameters.AddWithValue("@Titulo", certificado.Titulo);
                cmd.Parameters.AddWithValue("@Anexo", certificado.Anexo);
                cmd.Parameters.AddWithValue("@Url", certificado.Url);
                cmd.Parameters.AddWithValue("@DataCadastro", certificado.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Certificado registrado com sucesso!");
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

        public static JsonObject ConsultarCertificado(CandidatoCertificado certificado)
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
                cmd.CommandText = "SELECT Instituicao, Titulo, Anexo, Url, DATE_FORMAT(DataCadastro, '%Y-%m-%d %H:%i:%s.%f') AS DataCadastro FROM candidatos_certificados WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", certificado.Candidato);
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonObject certificados = new JsonObject();
                certificados.Add("Candidato", certificado.Candidato);
                JsonArray certificadoArray = new JsonArray();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        JsonObject certificadoTemp = new JsonObject();
                        certificadoTemp.Add("Instituicao", executa.GetString("Instituicao"));
                        certificadoTemp.Add("Titulo", executa.GetString("Titulo"));
                        certificadoTemp.Add("Anexo", (!executa.IsDBNull(executa.GetOrdinal("Anexo"))) ? executa.GetString("Anexo") : "");
                        certificadoTemp.Add("Url", (!executa.IsDBNull(executa.GetOrdinal("Url"))) ? executa.GetString("Url") : "");
                        certificadoTemp.Add("DataCadastro", executa.GetString("DataCadastro"));
                        certificadoArray.Add(certificadoTemp);
                    }
                    certificados.Add("Certificados", certificadoArray);
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("CONTEUDO", certificados);
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

        public static JsonObject ExcluirCertificado(CandidatoCertificado certificado)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM candidatos_certificados WHERE Candidato = @Candidato AND DataCadastro = @DataCadastro";
                cmd.Parameters.AddWithValue("@Candidato", certificado.Candidato);
                cmd.Parameters.AddWithValue("@DataCadastro", certificado.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Certificado deletado com sucesso!");
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
