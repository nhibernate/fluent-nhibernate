using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db;

[TestFixture]
public class PersistenceConfigurationTester
{
    #region Test Setup

    ConfigTester _config;
    Configuration _nhibConfig;

    [SetUp]
    public void SetUp()
    {
        _nhibConfig = null;
        _config = new ConfigTester();
    }

    public string ValueOf(string key)
    {
        if( _nhibConfig is null )
        {
            _nhibConfig = new Configuration();
            _config.ConfigureProperties(_nhibConfig);
        }

        return _nhibConfig.GetProperty(key);
    }
    #endregion

    [Test]
    public void ConfigureProperties_should_not_override_values_already_set()
    {
        _nhibConfig = new Configuration();
        _nhibConfig.Properties["proxyfactory.factory_class"] = "foo";
        ValueOf("proxyfactory.factory_class").ShouldEqual("foo");
           
    }

    [Test]
    public void Setting_raw_values_should_populate_dictionary()
    {
        _config.Raw("TESTKEY", "TESTVALUE");

        ValueOf("TESTKEY").ShouldEqual("TESTVALUE");
    }

    [Test]
    public void Dialect_should_set_both_old_and_new_dialect_keys()
    {
        _config.Dialect("foo");

        ValueOf("dialect").ShouldEqual("foo");
        ValueOf("hibernate.dialect").ShouldEqual("foo");
    }

    [Test]
    public void Default_Schema_should_be_set_to_schema_name()
    {
        _config.DefaultSchema("foo");
        ValueOf("default_schema").ShouldEqual("foo");
    }

    [Test]
    public void UseOuterJoin_should_set_value_to_const_true()
    {
        _config.UseOuterJoin();
        ValueOf("use_outer_join").ShouldEqual("true");
    }

    [Test]
    public void Max_Fetch_Depth_should_set_property_value()
    {
        _config.MaxFetchDepth(2);
        ValueOf("max_fetch_depth").ShouldEqual("2");
    }

    [Test]
    public void Use_Reflection_Optimizer_should_set_value_to_const_true()
    {
        _config.UseReflectionOptimizer();
        ValueOf("use_reflection_optimizer").ShouldEqual("true");

    }

    [Test]
    public void Query_Substitutions_should_set_property_value()
    {
        _config.QuerySubstitutions("foo");
        ValueOf("query.substitutions").ShouldEqual("foo");
    }
        
    [Test]
    public void Show_Sql_should_set_value_to_const_true()
    {
        _config.ShowSql();
        ValueOf("show_sql").ShouldEqual("true");
    }

    [Test]
    public void Format_Sql_should_set_value_to_const_true()
    {
        _config.FormatSql();
        ValueOf("format_sql").ShouldEqual("true");
    }

    [Test]
    public void DoNot_ShowSql_should_set_the_property_to_const_false()
    {
        _config.DoNot.ShowSql();
        ValueOf("show_sql").ShouldEqual("false");            
    }

    [Test]
    public void DoNot_Should_only_affect_next_property()
    {
        _config.DoNot.ShowSql().UseReflectionOptimizer();
        ValueOf("show_sql").ShouldEqual("false");
        ValueOf("use_reflection_optimizer").ShouldEqual("true");
    }
        
    [Test]
    public void Repeated_DoNot_Calls()
    {
        _config.DoNot.ShowSql().DoNot.UseReflectionOptimizer();
        ValueOf("show_sql").ShouldEqual("false");
        ValueOf("use_reflection_optimizer").ShouldEqual("false");
    }

    [Test]
    public void DoNot_UseReflectionOptimizer_should_set_the_property_to_const_false()
    {
        _config.DoNot.UseReflectionOptimizer();
        ValueOf("use_reflection_optimizer").ShouldEqual("false");
    }

    [Test]
    public void DoNot_UseOuterJoin_should_set_the_property_to_const_false()
    {
        _config.DoNot.UseOuterJoin();
        ValueOf("use_outer_join").ShouldEqual("false");
    }

    [Test]
    public void AdoNetBatchSize_should_set_property_value()
    {
        _config.AdoNetBatchSize(500);
        ValueOf("adonet.batch_size").ShouldEqual("500");
    }

    public class ConfigTester : PersistenceConfiguration<ConfigTester>
    {
    }
}
