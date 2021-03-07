using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TaxationQuerySystemAPI.Models;

namespace TaxationQuerySystemAPI.Services
{
    public class TaxTypeManager : XmlFileHandler<List<TaxType>>
    {
        public TaxTypeManager(IHostingEnvironment environment) : base(environment, "ArrayOfTaxType", "taxtype.xml")
        {

        }

        public void AddTaxType(TaxType newTaxType)
        {
            xmlObjects.Add(newTaxType);
        }

        public void EditTaxType(TaxType updatedTaxType)
        {
            int index = xmlObjects.FindIndex(m => m.Id == updatedTaxType.Id);
            TaxType[] taxtypeArr = xmlObjects.ToArray();
            taxtypeArr[index] = updatedTaxType;
            xmlObjects = taxtypeArr.ToList();
        }

        public void RemoveTaxType(TaxType taxType)
        {
            xmlObjects.Remove(taxType);
        }
    }
}
