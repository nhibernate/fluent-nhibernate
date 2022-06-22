using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.Hbm
{
    public class HbmConverterTestHelper
    {
        /// <summary>
        /// Test that a converter correctly handles the translation of a group of subobjects as an array which is broadly typed and can hold those subobjects.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the array of translated subobjects is narrowly typed for the subobjects (that is, the type stored in the array is
        /// exactly the subobject type, not an ancestor of it) then the
        /// <see cref="ShouldConvertSubobjectsAsStrictlyTypedArray{FMain, FSub, HMain, HSub}(Action{FMain, FSub}, Func{HMain, HSub[]})"/>
        /// method should be used in preference to this one.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// <see cref="HbmHibernateMappingConverter"/> converts from <see cref="HibernateMapping"/> to <see cref="HbmMapping"/>. The following test checks that the 
        /// <see cref="HibernateMapping.Classes">class subobjects</see> are converted to <see cref="HbmMapping.Items"/> correctly:
        /// </para>
        /// <code>
        /// ShouldConvertSubobjectsAsLooselyTypedArray&lt;HibernateMapping, ImportMapping, HbmMapping, HbmImport, object&gt;(
        ///     (hibernateMapping, importMapping) =&gt; hibernateMapping.AddImport(importMapping),
        ///     hbmMapping =&gt; hbmMapping.import
        /// );
        /// </code>
        /// <para>
        /// Specifically, because the conversion target is typed as <c>object[]</c> rather than <c>HbmClass[]</c> (in this case because the mapping XML specification
        /// allows other entries to be included), this method must be used rather than 
        /// <see cref="ShouldConvertSubobjectsAsStrictlyTypedArray{FMain, FSub, HMain, HSub}(Action{FMain, FSub}, Func{HMain, HSub[]})"/>.
        /// </para>
        /// </example>
        /// <seealso cref="ShouldConvertSubobjectsAsStrictlyTypedArray{FMain, FSub, HMain, HSub}(Action{FMain, FSub}, Func{HMain, HSub[]})"/>
        /// <typeparam name="FMain">the fluent type under test</typeparam>
        /// <typeparam name="FSub">the fluent subobject type under test</typeparam>
        /// <typeparam name="HMain">the translated (Hibernate) type</typeparam>
        /// <typeparam name="HSub">the translated (Hibernate) subobject type</typeparam>
        /// <typeparam name="HSubSuper">the type of the array which stores the translated subobjects</typeparam>
        /// <param name="addFSubToFMain">A handler which will add an fluent subobject to a fluent main object</param>
        /// <param name="getHSubSuperFromHMain">A handler which will retrieve an array of translated subobjects from the translated main object</param>
        public static void ShouldConvertSubobjectsAsLooselyTypedArray<FMain, FSub, HMain, HSub, HSubSuper>(Action<FMain, FSub> addFSubToFMain,
                Func<HMain, HSubSuper[]> getHSubSuperFromHMain)
            where FMain : IMapping, new()
            where FSub : IMapping, new()
            where HSub : HSubSuper, new()
        {
            // Set up a fake converter that registers any HSub instances it generates and returns in a list
            var generatedHSubs = new List<HSub>();
            var fakeConverter = A.Fake<IHbmConverter<FSub, HSub>>();
            A.CallTo(() => fakeConverter.Convert(A<FSub>.Ignored)).ReturnsLazily(fSub =>
            {
                var hSub = new HSub();
                generatedHSubs.Add(hSub);
                return hSub;
            });

            // Set up a custom container with the fake FSub->HSub converter registered, and obtain our main converter from it (so that it will use the fake implementation)
            var container = new HbmConverterContainer();
            container.Register<IHbmConverter<FSub, HSub>>(cnvrt => fakeConverter);
            IHbmConverter<FMain, HMain> converter = container.Resolve<IHbmConverter<FMain, HMain>>();

            // Allocate a new fluent main object instance, and add a subobject instance to it
            var fMain = new FMain();
            addFSubToFMain(fMain, new FSub());

            // Now try to convert it
            var convertedHMain = converter.Convert(fMain);

            // Finally, check that the array on the converted HMain instance which we expect to contain our converted HSub
            // // instances actually does, and that the converter was called the correct number of times
            getHSubSuperFromHMain(convertedHMain).ShouldEqual(generatedHSubs.ToArray());
            A.CallTo(() => fakeConverter.Convert(A<FSub>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        /// <summary>
        /// Test that a converter correctly handles the translation of a group of subobjects as an array which is narrowly typed to hold those subobjects.
        /// </summary>
        /// <remarks>
        /// If the array of translated subobjects is broadly typed for the subobjects (that is, the type stored in the array is
        /// an ancestor of the subobject type, rather than an exact match) then the
        /// <see cref="ShouldConvertSubobjectsAsLooselyTypedArray{FMain, FSub, HMain, HSub, HSubSuper}(Action{FMain, FSub}, Func{HMain, HSubSuper[]})"/>
        /// method must be used, rather than this one.
        /// <para>
        /// <see cref="HbmHibernateMappingConverter"/> converts from <see cref="HibernateMapping"/> to <see cref="HbmMapping"/>. The following test checks that the 
        /// <see cref="HibernateMapping.Imports">import subobjects</see> are converted to <see cref="HbmMapping.import"/> correctly:
        /// </para>
        /// <code>
        /// ShouldConvertSubobjectsAsStrictlyTypedArray&lt;HibernateMapping, ImportMapping, HbmMapping, HbmImport&gt;(
        ///     (hibernateMapping, importMapping) =&gt; hibernateMapping.AddImport(importMapping),
        ///     hbmMapping =&gt; hbmMapping.import
        /// );
        /// </code>
        /// <para>
        /// Specifically, because the conversion target is typed as <c>HbmImport[]</c> rather than some broader type (for example, <c>object[]</c>), this method should
        /// be used in preference to <see cref="ShouldConvertSubobjectsAsLooselyTypedArray{FMain, FSub, HMain, HSub, HSubSuper}(Action{FMain, FSub}, Func{HMain, HSubSuper[]})"/>.
        /// </para>
        /// </remarks>
        /// <example>
        /// </example>
        /// <seealso cref="ShouldConvertSubobjectsAsLooselyTypedArray{FMain, FSub, HMain, HSub, HSubSuper}(Action{FMain, FSub}, Func{HMain, HSubSuper[]})"/>
        /// <typeparam name="FMain">the fluent type under test</typeparam>
        /// <typeparam name="FSub">the fluent subobject type under test</typeparam>
        /// <typeparam name="HMain">the translated (Hibernate) type</typeparam>
        /// <typeparam name="HSub">the translated (Hibernate) subobject type</typeparam>
        /// <param name="addFSubToFMain">A handler which will add an fluent subobject to a fluent main object</param>
        /// <param name="getHSubFromHMain">A handler which will retrieve an array of translated subobjects from the translated main object</param>
        public static void ShouldConvertSubobjectsAsStrictlyTypedArray<FMain, FSub, HMain, HSub>(Action<FMain, FSub> addFSubToFMain, Func<HMain, HSub[]> getHSubFromHMain)
            where FMain : IMapping, new()
            where FSub : IMapping, new()
            where HSub : new()
        {
            // Strongly typed is just loosely typed with HSubSuper == HSub to restrict it to being exactly HSub
            ShouldConvertSubobjectsAsLooselyTypedArray<FMain, FSub, HMain, HSub, HSub>(addFSubToFMain, getHSubFromHMain);
        }
    }
}
