using LidaCarreiras.Funcoes;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.Json.Nodes;

namespace LidaCarreiras.Classes
{
    public class EmpresaVaga
    {
        private long Id { get; set; }
        private long? Empresa { get; set; }
        private string? Titulo { get; set; }
        private string? Descricao { get; set; }
        private long? Modalidade { get; set; }
        private long? Modelo { get; set; }
        private long? Area { get; set; }
        private string? DataLimite { get; set; }
        private bool? Status { get; set; }
        private string? DataCadastro { get; set; }

        public EmpresaVaga(long empresa, string titulo, string descricao, long modalidade, long modelo, long area, string dataLimite, bool status, string dataCadastro)
        {
            this.Empresa = empresa;
            this.Titulo = titulo;
            this.Descricao = descricao;
            this.Modalidade = modalidade;
            this.Modelo = modelo;
            this.Area = area;
            this.DataLimite = dataLimite;
            this.Status = status;
            this.DataCadastro = dataCadastro;
        }

        public EmpresaVaga(long id)
        {
            this.Empresa = id;
            this.Id = id;
        }

        public EmpresaVaga(long id, string titulo, string descricao, long modalidade, long modelo, long area, string dataLimite, bool status)
        {
            this.Id = id;
            this.Titulo = titulo;
            this.Descricao = descricao;
            this.Modalidade = modalidade;
            this.Modelo = modelo;
            this.Area = area;
            this.DataLimite = dataLimite;
            this.Status = status;
        }

        public static JsonObject RegistrarVaga(EmpresaVaga vaga)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO vagas (Empresa,Titulo,Descricao,Modalidade,Modelo,Area,DataLimite,Status,DataCadastro) VALUES (@Empresa,@Titulo,@Descricao,@Modalidade,@Modelo,@Area,@DataLimite,@Status,@DataCadastro)";
                cmd.Parameters.AddWithValue("@Empresa", vaga.Empresa);
                cmd.Parameters.AddWithValue("@Titulo", vaga.Titulo);
                cmd.Parameters.AddWithValue("@Descricao", vaga.Descricao);
                cmd.Parameters.AddWithValue("@Modalidade", vaga.Modalidade);
                cmd.Parameters.AddWithValue("@Modelo", vaga.Modelo);
                cmd.Parameters.AddWithValue("@Area", vaga.Area);
                cmd.Parameters.AddWithValue("@DataLimite", (vaga.DataLimite != "") ? vaga.DataLimite : null);
                cmd.Parameters.AddWithValue("@Status", vaga.Status);
                cmd.Parameters.AddWithValue("@DataCadastro", vaga.DataCadastro);
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

        public static JsonObject ConsultarVagas(EmpresaVaga vaga)
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
                cmd.CommandText = "SELECT * FROM vagas WHERE Empresa = @Empresa";
                cmd.Parameters.AddWithValue("@Empresa", vaga.Empresa);
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonObject vagas = new JsonObject();
                vagas.Add("Empresa", vaga.Empresa);
                JsonArray vagaArray = new JsonArray();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        JsonObject vagaTemp = new JsonObject();
                        vagaTemp.Add("Id", long.Parse(executa.GetString("Id")));
                        vagaTemp.Add("Titulo", executa.GetString("Titulo"));
                        vagaTemp.Add("Descricao", executa.GetString("Descricao"));
                        vagaTemp.Add("Modalidade", long.Parse(executa.GetString("Modalidade")));
                        vagaTemp.Add("Modelo", long.Parse(executa.GetString("Modelo")));
                        vagaTemp.Add("Area", long.Parse(executa.GetString("Area")));
                        vagaTemp.Add("DataLimite", (!executa.IsDBNull(executa.GetOrdinal("DataLimite"))) ? DateTime.ParseExact(executa.GetString("DataLimite"), "dd/MM/yyyy HH:mm:ss", culture).ToString("dd/MM/yyyy") : "");
                        vagaTemp.Add("DataCadastro", DateTime.ParseExact(executa.GetString("DataCadastro"), "dd/MM/yyyy HH:mm:ss", culture).ToString("dd/MM/yyyy"));
                        vagaTemp.Add("Status", Convert.ToBoolean(executa.GetString("Status")));
                        vagaArray.Add(vagaTemp);
                    }
                    vagas.Add("Vagas", vagaArray);
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("CONTEUDO", vagas);
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

        public static JsonObject EditarVaga(EmpresaVaga vaga)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "UPDATE vagas SET Titulo = @Titulo,Descricao = @Descricao,Modalidade = @Modalidade,Modelo = @Modelo,Area = @Area,DataLimite = @DataLimite,Status = @Status WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", vaga.Id);
                cmd.Parameters.AddWithValue("@Titulo", vaga.Titulo);
                cmd.Parameters.AddWithValue("@Descricao", vaga.Descricao);
                cmd.Parameters.AddWithValue("@Modalidade", vaga.Modalidade);
                cmd.Parameters.AddWithValue("@Modelo", vaga.Modelo);
                cmd.Parameters.AddWithValue("@Area", vaga.Area);
                cmd.Parameters.AddWithValue("@DataLimite", (vaga.DataLimite != "") ? vaga.DataLimite : null);
                cmd.Parameters.AddWithValue("@Status", vaga.Status);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Registro atualizado com sucesso!");
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

        public static JsonObject ExcluirVaga(EmpresaVaga vaga)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM vagas WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", vaga.Id);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Vaga excluída com sucesso!");
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
