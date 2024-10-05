using algıthm_proje;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

// Properties
public class Category
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }

    // Constructor
    public Category(int id ,string name,string desc)
    {
        CategoryID = id;
        CategoryName = name;
        Description = desc ;
    }
}


public class Customer
{
    public int CustomerID { get; set; }
    public string CustomerName { get; set; }
    public string ContactName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    public Customer(int customerID, string customerName, string contactName, string address, string city, string postalCode, string country)
    {
        CustomerID = customerID;
        CustomerName = customerName;
        ContactName = contactName;
        Address = address;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }
}

public class Employee
{
    public int EmployeeID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Photo { get; set; }
    public string Notes { get; set; }

    public Employee(int employeeID, string lastName, string firstName, DateTime birthDate, string photo, string notes)
    {
        EmployeeID = employeeID;
        LastName = lastName;
        FirstName = firstName;
        BirthDate = birthDate;
        Photo = photo;
        Notes = notes;
    }
}

public class Shipper
{
    public int ShipperID { get; set; }
    public string ShipperName { get; set; }
    public string Phone { get; set; }

    public Shipper(int shipperID, string shipperName, string phone)
    {
        ShipperID = shipperID;
        ShipperName = shipperName;
        Phone = phone;
    }
}

public class Supplier
{
    public int SupplierID { get; set; }
    public string SupplierName { get; set; }
    public string ContactName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }

    public Supplier(int supplierID, string supplierName, string contactName, string address, string city, string postalCode, string country, string phone)
    {
        SupplierID = supplierID;
        SupplierName = supplierName;
        ContactName = contactName;
        Address = address;
        City = city;
        PostalCode = postalCode;
        Country = country;
        Phone = phone;
    }
}
//  CompareTo method for sorting by price
public class Product : IComparable<Product>
{
    public int CompareTo(Product other)
    {
        
        return this.Price.CompareTo(other.Price);
    }
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public int SupplierID { get; set; }
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string Unit { get; set; }
    public decimal Price { get; set; }
    public Product(int productID, string productName, int supplierID, int categoryID,string categoryName, string unit, decimal price)
    {
        ProductID = productID;
        ProductName = productName;
        SupplierID = supplierID;
        CategoryID = categoryID;
        CategoryName = categoryName;
        Unit = unit;
        Price = price;
    }
}
// Class representing products and quantities in the shopping cart.

public class CartProduct  
{
    public int count;
    public Product product;


    // Quantity of the product in the cart and product information.

    public CartProduct(int count, Product product)
    {
        this.count = count;
        this.product = product;
    }

}

public class Order
{
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public int EmployeeID { get; set; }
    public DateTime OrderDate { get; set; }
    public int ShipperID { get; set; }

    public Order(int orderID, int customerID, int employeeID, DateTime orderDate, int shipperID)
    {
        OrderID = orderID;
        CustomerID = customerID;
        EmployeeID = employeeID;
        OrderDate = orderDate;
        ShipperID = shipperID;
    }
}

// Class controlling the initial state of the application.

public class InitEcommerce
{
    public Boolean product { get; set; }
    public Boolean category { get; set; }

    public  InitEcommerce () 
    { 
        this.category = true;
        this.product = true;
    }
}
// Total price of the cart and the list of cart products.

// Method for adding a product to the cart.
public class Cart
{
    public decimal totalPrice;
    public List<CartProduct> cartProducts;

    public Cart()
    {
        this.totalPrice = 0;
        this.cartProducts = new List<CartProduct>();
    }

    public void  AddToCart(Product product)
    {
        CartProduct findProduct =this.cartProducts.Find((x)=>x.product==product);
        this.totalPrice += product.Price;

        if (findProduct != null)
        {
            findProduct.count += 0;
        }
        else
        {
            this.cartProducts.Add(new CartProduct(1, product));
        }

    }
}

// Main class managing e-commerce site functionality.
// SQL connection object.
// Object controlling the initial state.
public class ECommerceSite
{
    SqlBaglanti sql = new SqlBaglanti();
    public  InitEcommerce initEcommerce=new InitEcommerce();

    // Lists holding categories, products, and the shopping cart.

    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Product> Products { get; set; } = new List<Product>();
    public Cart cart = new Cart();

    public ECommerceSite()
    {
        ViewMenu();
    }

    // Method selecting all categories.

    public void SelectAllCategories() 
    {
        if (!this.initEcommerce.category)
        {
            return;
        }
        SqlCommand komut2 = new SqlCommand("Select * from Categories ", sql.baglanti());
        SqlDataReader dr = komut2.ExecuteReader();
         
        while (dr.Read())
        {
            this.Categories.Add(new Category(Convert.ToInt32(dr["CategoryID"]) ,Convert.ToString(dr["CategoryName"]) ,Convert.ToString(dr["Description"]) ));
        }
        sql.baglanti().Close();
        this.initEcommerce.category = false;
    }
    public void SelectAllProducts()
    {
        if (!this.initEcommerce.product)
        {
            return;
        }
        SqlCommand komut2 = new SqlCommand("Select * from Products p INNER JOIN  Categories c ON p.CategoryID = c.CategoryID", sql.baglanti());
        SqlDataReader dr = komut2.ExecuteReader();

        while (dr.Read())
        {
            this.Products.Add(new Product (Convert.ToInt32(dr["ProductID"]), Convert.ToString(dr["ProductName"]), Convert.ToInt32((dr["SupplierID"])),Convert.ToInt32((dr["CategoryID"])), Convert.ToString(dr["CategoryName"]),
                Convert.ToString(dr["Unit"]),Convert.ToDecimal((dr["Price"]))));

        }
        sql.baglanti().Close();
        this.initEcommerce.product = false;
    }


    // Method displaying categories and managing the user interface.

    // Selects and lists all categories.
    // Waits for user input and performs actions based on the input.

    public void ViewCategories()
    {     
            SelectAllCategories();

        string choose;
        int intChoose;
            Console.WriteLine("---Category Menu---");

        Boolean init = true;
        do
        {
            if (!init)
            {
                Console.WriteLine("Wrong Choose. Please Try Again.");
            }
            else
            {
                init = false;
            }

            int index = 1;
            foreach (Category category in this.Categories)
            {
                Console.WriteLine($" {index}: {category.CategoryName}");
                index++;
            }
 
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Return Main Menu");

            choose = Console.ReadLine();
        }
        while (int.TryParse(choose, out intChoose) && intChoose <= 0 && intChoose > 1);

        switch (intChoose)
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                ViewMenu();
                break;
             

        }
    }

 public void AddToCart()
    // Prompts the user to enter the product ID
    {
        int productId;
    do
    {       

            Console.WriteLine("Enter the product ID to add to the cart:");
    } while (!int.TryParse(Console.ReadLine(), out productId) || !Products.Any(p => p.ProductID == productId));


        // Find the selected product by the user
        Product selectedProduct = Products.First(p => p.ProductID == productId);

        // Add the selected product to the cart

        cart.AddToCart(selectedProduct);
    Console.WriteLine($"Product '{selectedProduct.ProductName}' added to the cart.");

    Console.ReadLine();
}

    public void ViewProducts()
    {
        Boolean init = true;
        if (this.initEcommerce.product)
            SelectAllProducts();
        int choice;
        do
        {
            if (init)
            {
                init = false;
            }
            else
            {
                // Display a message for an invalid option
                Console.WriteLine("Invalid option. Please enter a valid option.");
            }
            Console.WriteLine("---View Products---");

            // Display a list of products with corresponding indices

            int index = 2;
            foreach (Product product in this.Products)
            {
                Console.WriteLine($" {index}: {product.ProductName}");
                index++;
            }
            Console.WriteLine("0.Exit");
            Console.WriteLine("1.Return Main Menu");
            
            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 2:
                    AddToCart(); 
                    break;
            }
        } while (choice < 0 && choice > this.Products.Count);

        if (choice != 1)
        {
            Product _product =  this.Products[choice - 2];
            Console.WriteLine($"Selected Product: {_product.ProductName} ");
            cart.AddToCart(_product);
            Console.WriteLine($"Added Product-> Name: {_product.ProductName}, Price: {_product.Price}");
            ViewMenu();
        }
    }

    public void ViewShoppingCart()
    {
        // Display the shopping cart header
        Console.WriteLine("---Shopping Cart---");
        // Check if the shopping cart is empty

        if (cart.cartProducts.Count == 0)
        {
            Console.WriteLine("Your shopping cart is empty.");
        }
        else
        {
            // Sort products in the shopping cart based on their prices

            cart.cartProducts.Sort((p1, p2) => p1.product.Price.CompareTo(p2.product.Price));

            // Display each product in the shopping cart


            foreach (var product in cart.cartProducts)
            {
                Console.WriteLine($"Count: {product.count}, Name: {product.product.ProductName}, Price: {product.product.Price}");
            }
        }

        // Display the total price of products in the shopping cart


        Console.WriteLine($"Total Price: {cart.totalPrice}");

        // User menu for further actions

        string choose;
        int intChoose;
        Boolean init = true;
        do
        {
            if (!init)
            {
                Console.WriteLine("Wrong Choose. Please Try Again.");
            }
            else
            {
                init = false;
            }

            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Return Main Menu");

            choose = Console.ReadLine();
        }
        while (int.TryParse(choose, out intChoose) && intChoose <= 0 && intChoose > 1);

        // Process user choice


        switch (intChoose)
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                ViewMenu();
                break;
           
        }
    }

    public void ViewMenu()
    {
        Boolean init = true;
        // Initialization of variables

        int choice;
        do
        {
            if (init)
            {
                init = false;
            }
            else
            {
                Console.WriteLine("Invalid option. Please enter a valid option.");
            }

            // Display the main menu options


            Console.WriteLine("---Menu---");
            Console.WriteLine("1.View Categories");
            Console.WriteLine("2.View Products ");
            Console.WriteLine("3.Find Products");
            Console.WriteLine("4.View Shopping Cart"); 
            Console.WriteLine("0.Exit");

            // Get user choice


            Console.WriteLine("---Select Menu---");
            choice = Convert.ToInt32(Console.ReadLine());

            // Process user choice


            switch (choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    ViewCategories();
                    break;
                case 2:
                    ViewProducts();
                    break;
                case 3:
                    FindProducts();
                    break;
                case 4:
                    ViewShoppingCart(); 
                    break;
                default:
                    break;
            }
        } while (choice > 4 || choice < 0);
    }
    public void FindProducts()
    {
        // Display the find products menu options


        Console.WriteLine("---Find Products---");
        Console.WriteLine("1.Find by Product Name");
        Console.WriteLine("2.Find by Category Name");
        Console.WriteLine("0.Exit");

        Console.WriteLine("---Select Search Option---");
        int searchOption = Convert.ToInt32(Console.ReadLine());

        switch (searchOption)
        {
            case 1:

                SearchProductsByName();
                break;
            case 2:
                SearchProductsByCategory();
                break;
            case 0:
                break;
            default:
                Console.WriteLine("Invalid option. Please enter a valid option.");
                break;
        }
    }
    private void SearchProductsByName()
    {
        // Variables for product search


        List<Product> foundProducts;
        string choose;
        int intChoose;

        // Display all products to the user


        this.SelectAllProducts();
        Boolean init = true;
        do
        {
            if (!init)
            {
                Console.WriteLine("Wrong Choose. Please Try Again.");
            }
            else
            {
                init = false;
            }


            // Prompt user to enter the product name for search


            Console.WriteLine("Enter Product Name:");
            string productName = Console.ReadLine();


            // Perform case-insensitive search for products by name


            foundProducts = Products.FindAll(p => p.ProductName.ToLower().Contains(productName.ToLower()));

            if (foundProducts.Count > 0)
            {
                Console.WriteLine($"---Found Products with Name '{productName}'---");
                int index = 2;
                foreach (var product in foundProducts)
                {
                    Console.WriteLine($"{index}. Name: {product.ProductName}, Price: {product.Price}");
                    index++;
                }
            }
            else
            {
                Console.WriteLine($"No products found with the name '{productName}'.");
            }
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Return Main Menu");

            choose = Console.ReadLine();
        }
        while (int.TryParse(choose, out  intChoose) && intChoose <= 0 && intChoose >=(foundProducts.Count + 1));

        switch (intChoose)
        {
            case 0:
                    Environment.Exit(0);
                    break;
            case 1:
                ViewMenu();
                break;
            default:

                // Add the selected product to the shopping cart


                Product _product = foundProducts[intChoose- 2];
                cart.AddToCart( _product );
                Console.WriteLine($"Added Product-> Name: {_product.ProductName}, Price: {_product.Price}");
                ViewMenu();
                break;

        }
    }
    private void SearchProductsByCategory()
    {
        // Variables for product search

        List<Product> foundProducts;
        string choose;
        int intChoose;


        this.SelectAllProducts();
        Boolean init = true;
        do
        {
            if (!init)
            {
                Console.WriteLine("Wrong Choose. Please Try Again.");
            }
            else
            {
                init = false;
            }

            Console.WriteLine("Enter Category Name:");
            string categoryName = Console.ReadLine();

            // Perform case-insensitive search for products by category name


            foundProducts = Products.FindAll(p => p.CategoryName.ToLower().Contains(categoryName.ToLower()));

            if (foundProducts.Count > 0)
            {

                // Display found products in the specified category


                Console.WriteLine($"---Found Category with Name '{categoryName}'---");
                int index = 2;
                foreach (var product in foundProducts)
                {
                    Console.WriteLine($"{index}. Name: {product.ProductName}, Price: {product.Price}");
                    index++;
                }
            }
            else
            {
                Console.WriteLine($"No products found with the Category name '{categoryName}'.");
            }
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Return Main Menu");

            choose = Console.ReadLine();
        }
        while (int.TryParse(choose, out intChoose) && intChoose <= 0 && intChoose >= (foundProducts.Count + 1));

        switch (intChoose)
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                ViewMenu();
                break;
            default:

                // Add the selected product to the shopping cart


                Product _product = foundProducts[intChoose - 2];
                cart.AddToCart(_product);
                Console.WriteLine($"Added Product-> Name: {_product.ProductName}, Price: {_product.Price}");
                ViewMenu();
                break;

        }
    }
}

class Program
{
    static void Main()
    {
        ECommerceSite eCommerceSite = new ECommerceSite();

       
    }
   
}
