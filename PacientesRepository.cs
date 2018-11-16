using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Collections.Generic;

namespace NutriYA{
    public class PacientesRepository{
        public const string STORAGEACCOUNTNAME = "s101moyag8";
        public const string ACCOUNTKEY = "WPB64UdtcYgJZ+d+EQW8v+LPrj0YkakcAsQXtE6KvOhMaTxuIaP+EqD7tXHpG3hqoKMlAWFwdLR2e1vWU57i+g==";

        public List<Paciente> LeerPaciente(){
            var Table = ReferenciaTabla("Pacientes");
            List<Paciente> Pacientes = new List<Paciente>();
            TableQuery<PacienteEntity> query = new TableQuery<PacienteEntity>().Where
                (
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual," ")
                );            
                
                CacharPacientes();

                async void CacharPacientes(){
                var list = new List<PacienteEntity>();
                var tk = new TableContinuationToken();
                foreach (PacienteEntity entity in await Table.ExecuteQuerySegmentedAsync(query,tk)){
                    
                    Pacientes.Add(new Paciente(
                        entity.PartitionKey, 
                        entity.RowKey,
                        entity.Correo, 
                        entity.Edad,
                        entity.Altura,
                        entity.Peso,
                        entity.IMC,
                        entity.Alergias));
                }
            }

            
            return Pacientes;
        }

        public bool CrearPaciente(Paciente model){
            var Table = ReferenciaTabla("Pacientes");

            Table.ExecuteAsync(TableOperation.Insert(new PacienteEntity(model.NombreNut,model.NombrePac,model.Correo,model.Edad,model.Altura,model.Peso)));

            return true;
        }

/*
        public async void BorrarPaciente(string PK, string RK){
            var Table = ReferenciaTabla("Pacientes");

            TableOperation retrieveOperation = TableOperation.Retrieve<PacienteEntity>(PK, RK);
            TableResult retrievedResult = await Table.ExecuteAsync(retrieveOperation);
            PacienteEntity deleteEntity = (PacienteEntity)retrievedResult.Result;

            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                await Table.ExecuteAsync(deleteOperation);
            }
        }
        */

        public bool ActualizarPaciente(Paciente Paci)
        {
            var Table = ReferenciaTabla("Pacientes");

            TableOperation retrieveOperation = TableOperation.Retrieve<PacienteEntity>(Paci.NombreNut,Paci.NombrePac);

            EditarNutriologo();
                async void EditarNutriologo(){
                    TableResult retrievedResult = await Table.ExecuteAsync(retrieveOperation);
                    PacienteEntity editEntity = (PacienteEntity)retrievedResult.Result;
                    if (editEntity != null)
                    {
                        editEntity.Edad     = Paci.Edad;
                        editEntity.Altura   = Paci.Altura;
                        editEntity.Peso     = Paci.Peso;

                        TableOperation editOperation = TableOperation.Replace(editEntity);

                        await Table.ExecuteAsync(editOperation);
                    }

                }

            return true;
        }

        public bool BorrarPaciente(Paciente Paci){
            var Table = ReferenciaTabla("Pacientes");
            TableOperation retrieveOperation = TableOperation.Retrieve<PacienteEntity>(Paci.NombreNut, Paci.NombrePac);
            EliminarPaciente();
            async void EliminarPaciente(){
                TableResult retrievedResult = await Table.ExecuteAsync(retrieveOperation);
                PacienteEntity deleteEntity = (PacienteEntity)retrievedResult.Result;
                if(deleteEntity != null){
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                    await Table.ExecuteAsync(deleteOperation);
                }
            }
            return true;

        }

        public PacienteEntity PacEntTemp = new PacienteEntity();
        public Paciente LeerPorPKRK(string PK, string RK)
            {
                
                Paciente PacienteFin = new Paciente();
                var Table = ReferenciaTabla("Pacientes");

                TableOperation retrieveOperation = TableOperation.Retrieve<PacienteEntity>(PK, RK);

                CacharPaciente();
                
                async void CacharPaciente(){
                    TableResult retrievedResult = await Table.ExecuteAsync(retrieveOperation);
                    var TempPac = (PacienteEntity)retrievedResult.Result;

                    var PaciFin = new Paciente(
                        TempPac.PartitionKey,
                        TempPac.RowKey,
                        TempPac.Correo,
                        TempPac.Edad,
                        TempPac.Altura,
                        TempPac.Peso,
                        TempPac.IMC,
                        TempPac.Alergias);
                    PacienteFin = PaciFin;
                }
               System.Threading.Thread.Sleep(500);
               return PacienteFin;   
            }

        
        public CloudTable ReferenciaTabla(string nombreTabla){
            
            var StorageAccount = new CloudStorageAccount(new StorageCredentials(STORAGEACCOUNTNAME,ACCOUNTKEY),false);
            var tableClient = StorageAccount.CreateCloudTableClient();
            var Table = tableClient.GetTableReference(nombreTabla);

            return Table;
        }
    }

    public class PacienteEntity : TableEntity
    {
        public PacienteEntity() { }
        public string NombreNut => PartitionKey;
        public string NombrePac => RowKey;
        public string Correo {get; set;}
        public int Edad { get; set; }
        public int Altura { get; set; }
        public int Peso { get; set; }
        public double IMC { get; set; }
        public string Alergias { get; set; }

        public override string ToString() => $"{NombreNut} {NombrePac} {Correo} {Edad} {Altura} {Peso} {IMC} {Alergias}";

        public PacienteEntity(string nombreNut,string nombrePac,string correo, int edad, int altura, int peso)
        {

            PartitionKey = nombreNut;
            RowKey = nombrePac;
            Correo = correo;
            Edad = edad;
            Altura = altura;
            Peso = peso;
            var Wea = (double)Altura / 100;
            Alergias = "";
            IMC = (double)Peso / (Wea*Wea);

        }
        
        
       
    }


}