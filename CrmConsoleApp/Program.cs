using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.WebServiceClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CrmConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<string> list = new List<string>();
                int counter = 0;
                string line;

                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(@"c:\EnvironmentSecrets\Credentials.txt");
                while ((line = file.ReadLine()) != null)
                {
                    System.Console.WriteLine(line);
                    list.Add(line);
                    counter++;
                }
                file.Close();
                System.Console.WriteLine("There were {0} lines.", counter);
                // Suspend the screen.  
                //System.Console.ReadLine();
                if (!string.IsNullOrEmpty(list[0]) && !string.IsNullOrEmpty(list[1]) && !string.IsNullOrEmpty(list[2]) && !string.IsNullOrEmpty(list[3]))
                {
                    IOrganizationService organizationService = null;
                    organizationService = Helper.Connect(list[0], list[1], list[2], list[3]);
                    //EntityCollection entityCollection = GetScanActivities(organizationService);


                    //-----------------------------------



                    try
                    {

                        //91b8f290-eed0-ea11-a813-0022480078c7
                        //msdyn_workordertype
                        //Entity workordertype_ = organizationService.Retrieve("msdyn_workordertype", new Guid("91b8f290-eed0-ea11-a813-0022480078c7"), new Microsoft.Xrm.Sdk.Query.ColumnSet("mzk_duration"));
                        //if (workordertype_.Attributes.Contains("mzk_duration") && !string.IsNullOrEmpty(Convert.ToString(workordertype_.GetAttributeValue<int>("mzk_duration"))))
                        //{
                        //    int abc12345 = workordertype_.GetAttributeValue<int>("mzk_duration");
                        //}

                        Guid caseId = new Guid("fc271628-74f0-eb11-94ef-000d3a8747d2");
                        //fc271628-74f0-eb11-94ef-000d3a8747d2
                        DateTime today = DateTime.Now;

                        QueryExpression queryExpression = new QueryExpression("opportunity");
                        queryExpression.Criteria.AddCondition("mzk_case", ConditionOperator.Equal, caseId);
                        queryExpression.ColumnSet = new ColumnSet(false);

                        LinkEntity linkEntityMasterPathway = new LinkEntity("opportunity", "mzk_masterpathway", "mzk_masterpathway", "mzk_masterpathwayid", JoinOperator.Inner);
                        linkEntityMasterPathway.Columns = new ColumnSet(false);

                        LinkEntity linkEntityMasterPathwayVisitGeneration = new LinkEntity("mzk_masterpathway", "mzk_masterpathwayvisitgeneration", "mzk_masterpathwayid", "mzk_masterpathway", JoinOperator.Inner);
                        linkEntityMasterPathwayVisitGeneration.Columns = new ColumnSet("mzk_numberofvisitstogenerate", "mzk_visittype");
                        linkEntityMasterPathwayVisitGeneration.EntityAlias = "mzk_masterpathway";
                        LinkEntity linkEntityWorkOrderType = new LinkEntity("mzk_masterpathwayvisitgeneration", "msdyn_workordertype", "mzk_visittype", "msdyn_workordertypeid", JoinOperator.Inner);
                        linkEntityWorkOrderType.Columns = new ColumnSet("mzk_duration");
                        linkEntityWorkOrderType.EntityAlias = "msdyn_workordertype";

                        queryExpression.LinkEntities.Add(linkEntityMasterPathway);
                        linkEntityMasterPathway.LinkEntities.Add(linkEntityMasterPathwayVisitGeneration);
                        linkEntityMasterPathwayVisitGeneration.LinkEntities.Add(linkEntityWorkOrderType);
                        EntityCollection entityCollection = organizationService.RetrieveMultiple(queryExpression);
                        foreach (Entity entity in entityCollection.Entities)
                        {
                            int visitNumber = 1;
                            int visitDuration = 0;
                            Guid workOrderType = Guid.Empty;
                            string workOrderName = string.Empty;

                            if (entity.Attributes.Contains("msdyn_workordertype.mzk_duration"))
                            {
                                if (numberOfVisits > 0)
                                {
                                    visitDuration = (int)entity.GetAttributeValue<AliasedValue>("msdyn_workordertype.mzk_duration").Value; 
                                }
                            }
                        }
                    


                            QueryExpression resourceQuery = new QueryExpression("bookableresource");
                        resourceQuery.Criteria.AddCondition("bookableresourceid", ConditionOperator.Equal, new Guid("9a033c1f-7ceb-ea11-a815-000d3a86d6ba"));
                        resourceQuery.ColumnSet = new ColumnSet("bookableresourceid", "msdyn_primaryemail", "name", "mzk_firstname", "mzk_lastname", "mzk_gendervalue", "mzk_phone", "mzk_contracttype", "statecode",
                            "mzk_businessunit", "mzk_payrollnumberemployeeref", "mzk_address1line1", "mzk_city1", "mzk_postalcode1", "mzk_resourcerole", "mzk_primaryclinicalterritory", "mzk_previousprimaryclinicalterritory", "mzk_resourcestatus", "mzk_workmobilenumber");
                        LinkEntity bookableResourceCompetencies = new LinkEntity("bookableresource", "bookableresourcecharacteristic", "bookableresourceid", "resource", JoinOperator.LeftOuter)
                        {

                            LinkCriteria = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                            {
                                new ConditionExpression("statecode", ConditionOperator.Equal, 0),
                    }
                            },
                            Columns = new ColumnSet("characteristic", "mzk_startdate", "mzk_expirydate", "mzk_competencystatus"),
                            EntityAlias = "Competency",


                        };
                        FilterExpression filter = bookableResourceCompetencies.LinkCriteria.AddFilter(LogicalOperator.And);
                        FilterExpression filterExp = filter.AddFilter(LogicalOperator.Or);
                        filterExp.AddCondition("mzk_competencystatus", ConditionOperator.Equal, 275380000);//active
                        filterExp.AddCondition("mzk_competencystatus", ConditionOperator.Equal, 275380002);//expired
                        //resourceQuery.LinkCriteria.AddCondition("mzk_reasoncodeoption", ConditionOperator.Equal, 275380038);
                        LinkEntity bookableResourceUser = new LinkEntity("bookableresource", "systemuser", "userid", "systemuserid", JoinOperator.LeftOuter)
                        {
                            Columns = new ColumnSet("positionid", "internalemailaddress", "fullname"),
                            EntityAlias = "User"
                        };
                        resourceQuery.LinkEntities.Add(bookableResourceCompetencies);
                        resourceQuery.LinkEntities.Add(bookableResourceUser);
                        EntityCollection resourceEntityCollection = organizationService.RetrieveMultiple(resourceQuery);
                        var groupedResources = resourceEntityCollection.Entities.GroupBy(item => (item.GetAttributeValue<Guid>("bookableresourceid")));
                        foreach (var groupedResourcesCurrent in groupedResources)
                        {

                            foreach (Entity entity in groupedResourcesCurrent)
                            {
                            
                            }
                        }


                                QueryExpression query888 = new QueryExpression("msdyn_workorder");
                        query888.Criteria.AddCondition("msdyn_name", ConditionOperator.Equal, "def2f21f-837d-eb11-a812-0022481a8b97");
                        EntityCollection entityCollection2222 = organizationService.RetrieveMultiple(query888);

                        //working
                        Entity workOrder7777 = organizationService.Retrieve("msdyn_workorder", new Guid("def2f21f-837d-eb11-a812-0022481a8b97"), new Microsoft.Xrm.Sdk.Query.ColumnSet("msdyn_name"));


                        IntegrationErrorContract<IntegrationErrorDataContract> parameters = new IntegrationErrorContract<IntegrationErrorDataContract>();
                        IntegrationErrorDataContract abc = new IntegrationErrorDataContract();
                        parameters.Body = abc;

                        parameters.Body.ErrorCode = null;
                        //parameters.Body.ErrorStackTrace = null;
                        parameters.Service = "ClinicianSchedule";
                        parameters.Body.ReferenceId = "SO-000000374";
                        parameters.Body.ErrorSource = "CreateTask";
                        parameters.ServiceType = "BusinessProcessError";
                        parameters.Body.ErrorCode = "Click Error";
                        parameters.Body.ErrorStackTrace = "Click Error";
                        //VanScheduling

                        Entity IntegrationError = new Entity("mzk_integrationerror");

                    if (!string.IsNullOrEmpty(parameters.Service))
                    {
                        if (parameters.Service == "VanScheduling")
                        {
                            IntegrationError["mzk_entity"] = "msdyn_workorder";
                        }
                        else if (parameters.Service == "PhoneMessage")
                        {
                            IntegrationError["mzk_entity"] = "mzk_phonemessage";
                        }
                        else if (parameters.Service == "ClinicianSchedule")
                        {
                            if (parameters.Body.ErrorSource == "CreateTask" || parameters.Body.ErrorSource == "UpdateTask" || parameters.Body.ErrorSource == "CreateTaskResponse" || parameters.Body.ErrorSource == "UpdateTaskResponse")
                            {
                                IntegrationError["mzk_entity"] = "msdyn_workorder";
                            }
                            if (parameters.Body.ErrorSource == "CreateResourceResponse" || parameters.Body.ErrorSource == "UpdateResourceRespoonse" || parameters.Body.ErrorSource == "CreateResource" || parameters.Body.ErrorSource == "UpdateResource")
                            {
                                IntegrationError["mzk_entity"] = "bookableresource";
                            }
                        }
                        IntegrationError["mzk_service"] = parameters.Service;
                    }
                    if (!string.IsNullOrEmpty(parameters.Body.ErrorCode))
                        IntegrationError["mzk_errorcode"] = parameters.Body.ErrorCode;

                    if (!string.IsNullOrEmpty(parameters.Body.ErrorMessage))
                        IntegrationError["mzk_errormessage"] = parameters.Body.ErrorMessage;

                    if (!string.IsNullOrEmpty(parameters.Body.ErrorSource))
                        IntegrationError["mzk_errorsource"] = parameters.Body.ErrorSource;

                    if (!string.IsNullOrEmpty(parameters.Body.ErrorStackTrace))
                        IntegrationError["mzk_errorstacktrace"] = parameters.Body.ErrorStackTrace;

                    if (!string.IsNullOrEmpty(parameters.ServiceType))
                    {
                        if (parameters.ServiceType.Equals("BusinessProcessError"))
                        {
                            //IntegrationError["mzk_errortype"] = new OptionSetValue((int)OptionSets.mzk_IntegrationErrorType.BusinessProcessError);
                        }
                        if (parameters.ServiceType.Equals("400 Bad Request"))
                        {
                            //IntegrationError["mzk_errortype"] = new OptionSetValue((int)OptionSets.mzk_IntegrationErrorType._400BadRequest);
                        }
                        if (parameters.ServiceType.Equals("500 Internal Server"))
                        {
                            //IntegrationError["mzk_errortype"] = new OptionSetValue((int)OptionSets.mzk_IntegrationErrorType._500InternalServer);
                        }
                    }
                    if (!string.IsNullOrEmpty(parameters.Body.ReferenceId))
                        IntegrationError["mzk_referenceid"] = parameters.Body.ReferenceId;

                    if (!string.IsNullOrEmpty(parameters.TrackingId))
                        IntegrationError["mzk_trackingid"] = parameters.TrackingId;

                    if (!string.IsNullOrEmpty(parameters.Body.ReferenceNumber))
                    {
                        IntegrationError["mzk_referencenumber"] = parameters.Body.ReferenceNumber;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(parameters.Body.ReferenceId))
                        {
                            if (parameters.Service == "VanScheduling")
                            {
                                Entity workOrder = organizationService.Retrieve("msdyn_workorder", new Guid(parameters.Body.ReferenceId), new Microsoft.Xrm.Sdk.Query.ColumnSet("msdyn_name"));
                                if (workOrder.Attributes.Contains("msdyn_name") && !string.IsNullOrEmpty(workOrder.GetAttributeValue<string>("msdyn_name")))
                                {
                                    IntegrationError["mzk_referencenumber"] = workOrder.GetAttributeValue<string>("msdyn_name");
                                }
                            }
                        }
                    }

                    if (parameters.Service == "ClinicianSchedule" && (parameters.Body.ErrorSource == "CreateTask" || parameters.Body.ErrorSource == "UpdateTask"))
                    {
                        if (!string.IsNullOrEmpty(parameters.Body.ReferenceId))
                        {
                            IntegrationError["mzk_referencenumber"] = parameters.Body.ReferenceId;
                        }
                        QueryExpression query = new QueryExpression("msdyn_workorder");
                        query.Criteria.AddCondition("msdyn_name", ConditionOperator.Equal, parameters.Body.ReferenceId);
                        EntityCollection entityCollection45345 = organizationService.RetrieveMultiple(query);
                        if (entityCollection != null && entityCollection.Entities != null && entityCollection.Entities.Count > 0)
                        {
                            if (entityCollection.Entities[0].Id != Guid.Empty)
                            {
                                IntegrationError["mzk_referenceid"] = entityCollection.Entities[0].Id.ToString();
                            }
                        }
                        else
                        {
                            IntegrationError["mzk_referenceid"] = Guid.Empty.ToString();
                            if (parameters.Body.ErrorStackTrace.Equals("Click Synchronous Response Error"))
                            {
                                IntegrationError["mzk_errormessage"] = parameters.Body.ErrorMessage;
                            }
                            else
                            {
                                IntegrationError["mzk_errormessage"] = parameters.Body.ErrorMessage + " Work Order not found";
                            }
                        }
                    }

                    if (parameters.Service == "ClinicianSchedule" && (parameters.Body.ErrorSource == "CreateResource" || parameters.Body.ErrorSource == "UpdateResource" || parameters.Body.ErrorSource == "CreateResourceResponse" || parameters.Body.ErrorSource == "UpdateResourceResponse"))
                    {
                        if (!string.IsNullOrEmpty(parameters.Body.ReferenceId))
                        {
                            Entity bookableresource = organizationService.Retrieve("bookableresource", new Guid(parameters.Body.ReferenceId), new Microsoft.Xrm.Sdk.Query.ColumnSet("name"));
                            if (bookableresource.Attributes.Contains("name") && !string.IsNullOrEmpty(bookableresource.GetAttributeValue<string>("name")))
                            {
                                IntegrationError["mzk_referencenumber"] = bookableresource.GetAttributeValue<string>("name");
                            }
                        }
                    }

                    if (parameters.Body.ErrorStackTrace.Equals("Click Synchronous Response Error") || (parameters.Service == "ClinicianSchedule" && (parameters.Body.ErrorSource == "CreateTaskResponse" || parameters.Body.ErrorSource == "UpdateTaskResponse")))
                    {
                        if (!string.IsNullOrEmpty(parameters.Body.ReferenceId))
                        {
                            IntegrationError["mzk_referenceid"] = parameters.Body.ReferenceId;
                            Entity workOrder = organizationService.Retrieve("msdyn_workorder", new Guid(parameters.Body.ReferenceId), new Microsoft.Xrm.Sdk.Query.ColumnSet("msdyn_name"));
                            if (workOrder.Attributes.Contains("msdyn_name") && !string.IsNullOrEmpty(workOrder.GetAttributeValue<string>("msdyn_name")))
                            {
                                IntegrationError["mzk_referencenumber"] = workOrder.GetAttributeValue<string>("msdyn_name");
                            }
                        }
                        else
                        {
                            IntegrationError["mzk_referenceid"] = Guid.Empty.ToString();
                            IntegrationError["mzk_errormessage"] = parameters.Body.ErrorMessage + " Work Order not found";
                        }
                    }

                    if (parameters.Service == "ClinicianSchedule")
                    {
                            organizationService.Create(IntegrationError);
                        //return string.Empty;
                    }
                    var request = new ExecuteMultipleRequest()
                    {
                        Requests = new OrganizationRequestCollection(),
                        Settings = new ExecuteMultipleSettings
                        {
                            ContinueOnError = true,
                            ReturnResponses = false
                        }
                    };
                    var createRequest = new CreateRequest()
                    {
                        Target = IntegrationError
                    };
                    request.Requests.Add(createRequest);
                    var response = (ExecuteMultipleResponse)organizationService.Execute(request);
                    //return string.Empty;
                }
                catch (Exception ex)
                {
                    throw ex;
                }



                    //====================================
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static EntityCollection GetScanActivities(IOrganizationService organizationService)
        {
            //mzk_scanningactivity
            QueryExpression query = new QueryExpression("mzk_scanningactivity");
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            query.Criteria.AddCondition("statuscode", ConditionOperator.Equal, 1);
            query.Criteria.AddCondition("mzk_batchclassname", ConditionOperator.NotEqual, 275380012);
            query.Criteria.AddCondition("mzk_batchclassname", ConditionOperator.NotEqual, 275380013);
            query.ColumnSet = new ColumnSet("mzk_batchclassname", "subject");

            LinkEntity annotationPDF = new LinkEntity("mzk_scanningactivity", "annotation", "activityid", "objectid", JoinOperator.Inner)
            {
                LinkCriteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("filename",ConditionOperator.Like,"%pdf%")
                    }
                }
            };
            LinkEntity annotationXML = new LinkEntity("mzk_scanningactivity", "annotation", "activityid", "objectid", JoinOperator.Inner)
            {
                LinkCriteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("filename",ConditionOperator.Like,"%xml%")
                    }
                }
            };
            query.LinkEntities.Add(annotationPDF);
            query.LinkEntities.Add(annotationXML);


            //if (scanActivityBatchCount != 0)
            //{
            //    query.TopCount = scanActivityBatchCount;
            //}
            //else
            //{
            //    query.TopCount = 5;
            //}
            EntityCollection entityCollection = organizationService.RetrieveMultiple(query);
            return entityCollection;
        }
    }
}
