using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;
namespace script
{
    class Program
    {
        static void writeDivisionDistrictUpazilaTODB()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Anindya\AppData\Roaming\Skype\My Skype Received Files\zone_dis_upazila.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            string connectionString = "server=172.16.26.87;database=Test;uid=user_prisonlink;pwd=User@pr1sonl1nk;";

            MySqlConnection cnn;
            cnn = new MySqlConnection(connectionString);
            zone zoneEntity = new script.zone();
            district districtEntity = new script.district();
            upazila upazilaEntity = new script.upazila();

            try
            {
                //cnn.Open();
                Console.WriteLine("Connection Open ! ");

                int row = 2272, column = 3;
                string zone = "", district = "", upazila = "";
                string lastUpazila = "";
                bool flag = false;
                int a = 0,rowInserted=0;
                for (int i = 2; i <= row; i++)
                {
                    for (int j = 1; j <= column; j++)
                    {
                        //new line
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            if (j == 1)
                                //Console.Write("\r\n");
                                zone = xlRange.Cells[i, j].Value2.ToString();
                            else if (j == 2)
                            {
                                district = xlRange.Cells[i, j].Value2.ToString();
                            }
                            else if (j == 3)
                            {
                                upazila = xlRange.Cells[i, j].Value2.ToString();
                            }
                        //write the value to the console

                        //Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");


                        //add useful things here!   
                    }
                    

                    TestEntities db = new TestEntities();
                    var zoneList = db.zones.Where(x => x.name == zone).OrderBy(x => x.id).ToList();
                    var districtList = db.districts.Where(x => x.name == district).OrderBy(x => x.id).ToList();
                    var upazilaList = db.upazilas.Where(x => x.name == upazila).ToList();

                    if (lastUpazila == upazila)
                        continue;

                    if (upazilaList.Count <= 0)
                    {
                        if (districtList.Count <= 0)
                        {
                            zoneEntity.name = zone.ToLower();
                            districtEntity.name = district.ToLower();

                            if (zoneList.Count <= 0)
                                districtEntity.zone = zoneEntity;
                            else
                                districtEntity.zone_id = zoneList.ElementAt(0).id;

                            upazilaEntity.district = districtEntity;
                        }
                        else
                        {
                            upazilaEntity.district_id = districtList.ElementAt(0).id;
                        }

                        upazilaEntity.name = upazila.ToLower();
                        db.upazilas.Add(upazilaEntity);
                        a = db.SaveChanges();
                        rowInserted++;

                        zoneEntity = new script.zone();
                        districtEntity = new script.district();
                        upazilaEntity = new script.upazila();
                    }
                    else
                    {
                        if (districtList.Count <= 0)
                        {
                            foreach (var item in upazilaList)
                            {
                                if (item.district.name != district)
                                {
                                    districtEntity.name = district.ToLower();
                                    upazilaEntity.district = districtEntity;
                                    flag = true;
                                }
                            }
                        }
                        else
                        {
                            foreach(var item in upazilaList)
                            {
                                if (item.district.name != district.ToLower())
                                {
                                    upazilaEntity.district_id = districtList.ElementAt(0).id;
                                    flag = true;
                                }
                            }
                        }

                        if (flag == true)
                        {
                            upazilaEntity.name = upazila.ToLower();
                            db.upazilas.Add(upazilaEntity);
                            a = db.SaveChanges();
                            rowInserted++;
                            flag = false;
                        }
                    }
                    lastUpazila = upazila;

                    if (a > 0)
                        Console.WriteLine(zone + "\t" + district + "\t" + upazila + "\t" + "Inserted");
                    else
                        Console.WriteLine(zone + "\t" + district + "\t" + upazila + "\t");

                }


                Console.WriteLine(rowInserted + " rows inserted");



                //cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection !  ");

            }

            Console.WriteLine("program terminated");
            Console.ReadKey();

        }
        static void DsheRecruitmentScriptForDivision()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Anindya\Downloads\142_final.xls");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            applicant_division divisionEntity = new applicant_division();
            applicant_district districtEntity = new applicant_district();
            applicant_upazila upazilaEntity = new applicant_upazila();

            try
            {
                //cnn.Open();
                Console.WriteLine("Connection Open ! ");

                int row = 142, column = 3;
                string division = "", district = "", upazila = "";
                string lastDistrict = "", lastDvision = "";
                bool flag = false;
                int a = 0, rowInserted = 0;
                for (int i = 2; i <= row; i++)
                {
                    for (int j = 1; j <= column; j++)
                    {
                        //new line
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            if (j == 1)
                                //Console.Write("\r\n");
                                division = xlRange.Cells[i, j].Value2.ToString();
                            else if (j == 2)
                            {
                                district = xlRange.Cells[i, j].Value2.ToString();
                            }
                            else if (j == 3)
                            {
                                upazila = xlRange.Cells[i, j].Value2.ToString();
                            }
                        //write the value to the console

                        //Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");


                        //add useful things here!   
                    }

                    dshe_reqruitmentEntitiesNew db = new dshe_reqruitmentEntitiesNew();

                    var divisionList = db.applicant_division.Where(x => x.name == division).OrderBy(x => x.id).ToList();
                    var districtList = db.applicant_district.Where(x => x.name == district).OrderBy(x => x.id).ToList();
                    var upazilaList = db.applicant_upazila.Where(x => x.name == upazila).ToList();

                    if (lastDistrict == upazila)
                        continue;

                    if (upazilaList.Count <= 0)
                    {
                        if (districtList.Count <= 0)
                        {
                            divisionEntity.name = division.ToLower();
                            districtEntity.name = district.ToLower();

                            if (divisionList.Count <= 0)
                                districtEntity.applicant_division = divisionEntity;
                            else
                                districtEntity.division_id = divisionList.ElementAt(0).id;

                            upazilaEntity.applicant_district = districtEntity;
                        }
                        else
                        {
                            upazilaEntity.district_id = districtList.ElementAt(0).id;
                        }

                        upazilaEntity.name = upazila.ToLower();
                        db.applicant_upazila.Add(upazilaEntity);
                        a = db.SaveChanges();
                        rowInserted++;

                        divisionEntity = new applicant_division();
                        districtEntity = new applicant_district();
                        upazilaEntity = new applicant_upazila();
                    }
                    else
                    {
                        if (districtList.Count <= 0)
                        {
                            foreach (var item in upazilaList)
                            {
                                if (item.applicant_district.name != district)
                                {
                                    districtEntity.name = district.ToLower();
                                    upazilaEntity.applicant_district = districtEntity;
                                    flag = true;
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in upazilaList)
                            {
                                if (item.applicant_district.name != district.ToLower())
                                {
                                    upazilaEntity.district_id = districtList.ElementAt(0).id;
                                    flag = true;
                                }
                            }
                        }

                        if (flag == true)
                        {
                            upazilaEntity.name = upazila.ToLower();
                            db.applicant_upazila.Add(upazilaEntity);
                            a = db.SaveChanges();
                            rowInserted++;
                            flag = false;
                        }
                    }
                    lastDistrict = upazila;

                    if (a > 0)
                        Console.WriteLine(division + "\t" + district + "\t" + upazila + "\t" + "Inserted");
                    else
                        Console.WriteLine(division + "\t" + district + "\t" + upazila + "\t");

                }


                Console.WriteLine(rowInserted + " rows inserted");



                //cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection !  ");

            }

            Console.WriteLine("program terminated");
            Console.ReadKey();

        }
        static void DsheRecruitmentScriptForDivisionNew()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Anindya\Documents\visual studio 2015\Projects\script\script\List_of_Upazila-23oct2017.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            string connectionString = "server=172.16.26.87;database=Test;uid=user_prisonlink;pwd=User@pr1sonl1nk;";

            MySqlConnection cnn;
            cnn = new MySqlConnection(connectionString);
            applicant_division divisionEntity = new applicant_division();
            applicant_district districtEntity = new applicant_district();
            applicant_upazila upazilaEntity = new applicant_upazila();

            try
            {
                //cnn.Open();
                Console.WriteLine("Connection Open ! ");

                int row = 491, column = 5;
                string division = "", district = "", upazila = "";
                int isApplicable =0;
                string lastUpazila = "";
                bool flag = false;
                int a = 0, rowInserted = 0;
                for (int i = 2; i <= row; i++)
                {
                    for (int j = 1; j <= column; j++)
                    {
                        //new line
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            if (j == 2)
                                //Console.Write("\r\n");
                                division = xlRange.Cells[i, j].Value2.ToString();
                            else if (j == 3)
                            {
                                district = xlRange.Cells[i, j].Value2.ToString();
                            }
                            else if (j == 4)
                            {
                                upazila = xlRange.Cells[i, j].Value2.ToString();
                            }
                            else if (j == 5)
                            {
                                isApplicable = int.Parse(xlRange.Cells[i, j].Value2.ToString());
                            }
                        //write the value to the console

                        //Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");

                        //add useful things here!   
                    }


                    //Console.WriteLine(division + "/t" + district + "/t" + upazila+ "/t" + isApplicable);

                    dshe_reqruitmentEntitiesNew db = new dshe_reqruitmentEntitiesNew();
                    var divisionList = db.applicant_division.Where(x => x.name == division).OrderBy(x => x.id).ToList();
                    var districtList = db.applicant_district.Where(x => x.name == district).OrderBy(x => x.id).ToList();
                    var upazilaList = db.applicant_upazila.Where(x => x.name == upazila).ToList();

                    if (lastUpazila == upazila)
                        continue;

                    if (upazilaList.Count <= 0)
                    {
                        if (districtList.Count <= 0)
                        {
                            divisionEntity.name = division.ToLower();
                            districtEntity.name = district.ToLower();

                            if (divisionList.Count <= 0)
                                districtEntity.applicant_division = divisionEntity;
                            else
                                districtEntity.division_id = divisionList.ElementAt(0).id;

                            upazilaEntity.applicant_district = districtEntity;
                        }
                        else
                        {
                            upazilaEntity.district_id = districtList.ElementAt(0).id;
                        }

                        upazilaEntity.name = upazila.ToLower();
                        upazilaEntity.is_applicable = isApplicable;
                        db.applicant_upazila.Add(upazilaEntity);
                        a = db.SaveChanges();
                        rowInserted++;

                        
                    }
                    else
                    {
                        //if same upazilla already exists
                        if (districtList.Count <= 0)
                        {
                            divisionEntity.name = division.ToLower();
                            districtEntity.name = district.ToLower();

                            if (divisionList.Count <= 0)
                                districtEntity.applicant_division = divisionEntity;
                            else
                                districtEntity.division_id = divisionList.ElementAt(0).id;

                            upazilaEntity.applicant_district = districtEntity;

                            upazilaEntity.name = upazila.ToLower();
                            upazilaEntity.is_applicable = isApplicable;


                            db.applicant_upazila.Add(upazilaEntity);
                            a = db.SaveChanges();
                            rowInserted++;

                        }
                        else if(districtList.Count >= 1 && districtList.ElementAt(0).name != upazilaList.ElementAt(0).applicant_district.name)
                        {
                            applicant_division div = (applicant_division)db.applicant_division.Where(x => x.name == division).First();
                            applicant_district dist = (applicant_district)db.applicant_district.Where(x => x.name == district).First();
                            
                            if (dist == null && div == null)
                            {
                                //creating new district and division
                                upazilaEntity.applicant_district = new applicant_district() { name = district, applicant_division = new applicant_division() { name = division } };

                            }
                            else if (dist != null && div != null)
                            {
                                upazilaEntity.applicant_district = (applicant_district)dist;
                            }

                            upazilaEntity.name = upazila.ToLower();
                            upazilaEntity.is_applicable = isApplicable;


                            db.applicant_upazila.Add(upazilaEntity);
                            a = db.SaveChanges();
                            rowInserted++;
                        }

                        
                    }

                    divisionEntity = new applicant_division();
                    districtEntity = new applicant_district();
                    upazilaEntity = new applicant_upazila();

                    lastUpazila = upazila;

                    if (a > 0)
                    {
                        Console.WriteLine(division + "\t" + district + "\t" + upazila + "\t" + "Inserted");
                    }
                    else
                        Console.WriteLine(division + "\t" + district + "\t" + upazila + "\t");

                }


                Console.WriteLine(rowInserted + " rows inserted");



                //cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);

            }

            Console.WriteLine("program terminated");
            Console.ReadKey();

        }
        static void DsheRecruitmentScriptForPostCode()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Anindya\Documents\visual studio 2015\Projects\script\script\Post code.xls");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            applicant_division divisionEntity = new applicant_division();
            applicant_district districtEntity = new applicant_district();
            applicant_upazila upazilaEntity = new applicant_upazila();

            try
            {
                //cnn.Open();
                Console.WriteLine("Connection Open ! ");

                int row = 491, column = 5;
                string division = "", district = "", upazila = "";
                int isApplicable = 0;
                string lastUpazila = "";
                bool flag = false;
                int a = 0, rowInserted = 0;
                for (int i = 2; i <= row; i++)
                {
                    for (int j = 1; j <= column; j++)
                    {
                        //new line
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            if (j == 2)
                                //Console.Write("\r\n");
                                division = xlRange.Cells[i, j].Value2.ToString();
                            else if (j == 3)
                            {
                                district = xlRange.Cells[i, j].Value2.ToString();
                            }
                            else if (j == 4)
                            {
                                upazila = xlRange.Cells[i, j].Value2.ToString();
                            }
                            else if (j == 5)
                            {
                                isApplicable = int.Parse(xlRange.Cells[i, j].Value2.ToString());
                            }
                        //write the value to the console

                        //Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");

                        //add useful things here!   
                    }


                    //Console.WriteLine(division + "/t" + district + "/t" + upazila+ "/t" + isApplicable);

                    dshe_reqruitmentEntitiesNew db = new dshe_reqruitmentEntitiesNew();
                    var divisionList = db.applicant_division.Where(x => x.name == division).OrderBy(x => x.id).ToList();
                    var districtList = db.applicant_district.Where(x => x.name == district).OrderBy(x => x.id).ToList();
                    var upazilaList = db.applicant_upazila.Where(x => x.name == upazila).ToList();

                    if (lastUpazila == upazila)
                        continue;

                    if (upazilaList.Count <= 0)
                    {
                        if (districtList.Count <= 0)
                        {
                            divisionEntity.name = division.ToLower();
                            districtEntity.name = district.ToLower();

                            if (divisionList.Count <= 0)
                                districtEntity.applicant_division = divisionEntity;
                            else
                                districtEntity.division_id = divisionList.ElementAt(0).id;

                            upazilaEntity.applicant_district = districtEntity;
                        }
                        else
                        {
                            upazilaEntity.district_id = districtList.ElementAt(0).id;
                        }

                        upazilaEntity.name = upazila.ToLower();
                        upazilaEntity.is_applicable = isApplicable;
                        db.applicant_upazila.Add(upazilaEntity);
                        a = db.SaveChanges();
                        rowInserted++;


                    }
                    else
                    {
                        //if same upazilla already exists
                        if (districtList.Count <= 0)
                        {
                            divisionEntity.name = division.ToLower();
                            districtEntity.name = district.ToLower();

                            if (divisionList.Count <= 0)
                                districtEntity.applicant_division = divisionEntity;
                            else
                                districtEntity.division_id = divisionList.ElementAt(0).id;

                            upazilaEntity.applicant_district = districtEntity;

                            upazilaEntity.name = upazila.ToLower();
                            upazilaEntity.is_applicable = isApplicable;


                            db.applicant_upazila.Add(upazilaEntity);
                            a = db.SaveChanges();
                            rowInserted++;

                        }
                        else if (districtList.Count >= 1 && districtList.ElementAt(0).name != upazilaList.ElementAt(0).applicant_district.name)
                        {
                            applicant_division div = (applicant_division)db.applicant_division.Where(x => x.name == division).First();
                            applicant_district dist = (applicant_district)db.applicant_district.Where(x => x.name == district).First();

                            if (dist == null && div == null)
                            {
                                //creating new district and division
                                upazilaEntity.applicant_district = new applicant_district() { name = district, applicant_division = new applicant_division() { name = division } };

                            }
                            else if (dist != null && div != null)
                            {
                                upazilaEntity.applicant_district = (applicant_district)dist;
                            }

                            upazilaEntity.name = upazila.ToLower();
                            upazilaEntity.is_applicable = isApplicable;


                            db.applicant_upazila.Add(upazilaEntity);
                            a = db.SaveChanges();
                            rowInserted++;
                        }


                    }

                    divisionEntity = new applicant_division();
                    districtEntity = new applicant_district();
                    upazilaEntity = new applicant_upazila();

                    lastUpazila = upazila;

                    if (a > 0)
                    {
                        Console.WriteLine(division + "\t" + district + "\t" + upazila + "\t" + "Inserted");
                    }
                    else
                        Console.WriteLine(division + "\t" + district + "\t" + upazila + "\t");

                }


                Console.WriteLine(rowInserted + " rows inserted");



                //cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);

            }

            Console.WriteLine("program terminated");
            Console.ReadKey();

        }

        static void DsheRecruitmentScriptForAddPublicUniversity()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Anindya\Downloads\Private Public Universities.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;


            university universityEntity = new university();
            dshe_reqruitmentEntitiesNew db = new dshe_reqruitmentEntitiesNew();

            try
            {
                //cnn.Open();
                Console.WriteLine("Connection Open ! ");

                int row = 40, column = 2;
                string university = "";
                string lastDistrict = "", lastDvision = "";
                bool flag = false;
                int a = 0, rowInserted = 0;
                for (int i = 1; i <= row; i++)
                {
                    for (int j = 1; j <= column; j++)
                    {
                        //new line
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            if (j == 2)
                            {
                                university = xlRange.Cells[i, j].Value2.ToString();
                            }

                        //write the value to the console



                        //add useful things here!   
                    }

                    university = university.Replace(@"*", string.Empty);
                    Console.WriteLine(university);
                    universityEntity.name = university.Trim().ToLower();
                    universityEntity.type = 1;
                    db.universities.Add(universityEntity);
                    
                    rowInserted++;

                    universityEntity = new script.university();
                }
                db.SaveChanges();

                Console.WriteLine(rowInserted + " rows inserted");



                //cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection !  ");

            }

            Console.WriteLine("program terminated");
            Console.ReadKey();

        }
        static void DsheRecruitmentScriptForAddPrivatecUniversity() {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Anindya\Downloads\Private Public Universities.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[2];
            Excel.Range xlRange = xlWorksheet.UsedRange;


            university universityEntity = new university();
            dshe_reqruitmentEntitiesNew db = new dshe_reqruitmentEntitiesNew();

            try
            {
                //cnn.Open();
                Console.WriteLine("Connection Open ! ");

                int row = 95, column = 2;
                string university = "";
                int a = 0, rowInserted = 0;
                for (int i = 1; i <= row; i++)
                {
                    for (int j = 1; j <= column; j++)
                    {
                        //new line
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            if (j == 2)
                            {
                                university = xlRange.Cells[i, j].Value2.ToString();
                            }

                    }
                    university = university.Replace(@"*", string.Empty);
                    Console.WriteLine(university);
                    universityEntity.name = university.Trim().ToLower();
                    universityEntity.type = 2;
                    db.universities.Add(universityEntity);

                    rowInserted++;

                    universityEntity = new script.university();
                }

                db.SaveChanges();
                Console.WriteLine(rowInserted + " rows inserted");



                //cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection !  ");

            }

            Console.WriteLine("program terminated");
            Console.ReadKey();
        }

        static void test1()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Anindya\AppData\Roaming\Skype\My Skype Received Files\zone_dis_upazila.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            string connectionString = "server=172.16.26.87;database=Test;uid=user_prisonlink;pwd=User@pr1sonl1nk;";

            MySqlConnection cnn;
            cnn = new MySqlConnection(connectionString);
            zone zoneEntity = new script.zone();
            district districtEntity = new script.district();
            upazila upazilaEntity = new script.upazila();

            try
            {
                //cnn.Open();
                Console.WriteLine("Connection Open ! ");

                int row = 2272, column = 3;
                string zone = "", district = "", upazila = "";
                string lastUpazila = "";
                bool isZoneExists = false;
                bool isDistrictExists = false;
                bool isUpazilaExists = false;
                for (int i = 2; i <= row; i++)
                {
                    for (int j = 1; j <= column; j++)
                    {
                        //new line
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            if (j == 1)
                                //Console.Write("\r\n");
                                zone = xlRange.Cells[i, j].Value2.ToString();
                            else if (j == 2)
                            {
                                district = xlRange.Cells[i, j].Value2.ToString();
                            }
                            else if (j == 3)
                            {
                                upazila = xlRange.Cells[i, j].Value2.ToString();
                            }
                        //write the value to the console

                        //Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");


                        //add useful things here!   
                    }
                    Console.WriteLine(zone + "\t" + district + "\t" + upazila + "\t");

                    TestEntities db = new TestEntities();
                    var zoneList = db.zones.Where(x => x.name == zone).OrderBy(x => x.id).ToList();
                    var districtList = db.districts.Where(x => x.name == district).OrderBy(x => x.id).ToList();
                    var upazilaList = db.upazilas.Where(x => x.name == upazila).ToList();
                    //district districtEntity = ee.districts.Where(x => x.name == district).FirstOrDefault();
                    //upazila upazilaEntity = ee.upazilas.Where(x => x.name == upazila).FirstOrDefault();
                    if (lastUpazila == upazila)
                        continue;

                    if (zoneList.Count <= 0)
                    {
                        zoneEntity.name = zone.ToLower();
                        //db.zones.Add(zoneEntity);
                    }


                    if (districtList.Count <= 0)
                    {
                        districtEntity.name = district.ToLower();
                        //districtEntity.zone_id = zoneList.FirstOrDefault()  ?? zoneEntity;
                        if (zoneEntity.name != null)
                            districtEntity.zone = zoneEntity;
                        else
                            districtEntity.zone_id = zoneList.FirstOrDefault().id;

                        //db.districts.Add(districtEntity);
                    }

                    if (upazilaList.Count <= 0 && lastUpazila != upazila)
                    {
                        upazilaEntity.name = upazila.ToLower();
                        if (districtEntity.name != null)
                            upazilaEntity.district = districtEntity;
                        else
                            upazilaEntity.district_id = districtList.FirstOrDefault().id;


                        db.upazilas.Add(upazilaEntity);
                    }
                    lastUpazila = upazila;

                    db.SaveChanges();


                    zoneEntity = new script.zone();
                    districtEntity = new script.district();
                    upazilaEntity = new script.upazila();

                }





                cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection !  ");

            }





            Console.WriteLine("program terminated");
            Console.ReadKey();

        }
        public static List<string> testGradation(string filename)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;


            try
            {
                int row = xlWorksheet.Rows.Count;
                List<string> IDs = new List<string>();
                for (int i = 1; i <= 5; i++)
                {

                    //new line
                    if (xlRange.Cells[i, 1] != null && xlRange.Cells[i, 1].Value2 != null)
                    {
                        IDs.Add(xlRange.Cells[i, 1].Value2.ToString());
                    }

                }
                return IDs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        static void Main(string[] args)
        {

            //test1();
            //writeDivisionDistrictUpazilaTODB();
            //DsheRecruitmentScriptForDivision();
            //DsheRecruitmentScriptForAddPublicUniversity();
            //DsheRecruitmentScriptForAddPrivatecUniversity();
            //DsheRecruitmentScriptForDivisionNew();
            testGradation(@"E:\Projects\script\script\gradationInput.xlsx");

        }
    }
}
