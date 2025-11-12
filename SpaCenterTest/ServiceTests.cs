using NUnit.Framework;
using SpaCenter;

namespace SpaCenterTest;

[TestFixture]
public class ServiceTests
{
    [Test]
    public void Constructor_Assigns_All_Properties()
    {
        var s = new Service("Massage", "Relax full body", 60, 100m, 16);
        Assert.That(s.Name, Is.EqualTo("Massage"));
        Assert.That(s.Description, Is.EqualTo("Relax full body"));
        Assert.That(s.Duration, Is.EqualTo(60));
        Assert.That(s.Price, Is.EqualTo(100m));
        Assert.That(s.MinimalAge, Is.EqualTo(16));
    }

    
    
        [Test]
        public void ViewServices_Returns_AllServices_List()
        {
            var s1 = new Service("Massage", "Relax full body", 60, 120m, 16);
            var s2 = new Service("Facial", "Skin treatment", 45, 80m, 14);
            

            var result = Service.ViewServices();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Does.Contain(s1));
            Assert.That(result, Does.Contain(s2));
        }
    

        
    }


    


