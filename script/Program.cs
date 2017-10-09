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
        static void test2()
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
        static void Main(string[] args)
        {

            //test1();
            test2();
        }
    }
}
