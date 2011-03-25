using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Visitors
{
    public class when_the_bi_directional_many_to_many_visitor_is_asked_to_pair_two_many_to_many_s_of_the_same_type_in_two_entities : BiDirectionalManyToManyPairingVisitorSpec
    {
        Establish context = () =>
        {
            members_in_queue = collection<Queue>(x => x.GetMembers());
            supervisors_in_queue = collection<Queue>(x => x.GetSupervisors());

            membership_queues_in_user = collection<User>(x => x.GetMembershipQueues());
            supervised_queues_in_user = collection<User>(x => x.GetSupervisedQueues());
        };

        Because of = () =>
            visit(members_in_queue, supervisors_in_queue, membership_queues_in_user, supervised_queues_in_user);

        It should_call_the_user_defined_func = () =>
            udf_was_called.ShouldBeTrue();

        It should_set_other_side_for_the_members_collection = () =>
            members_in_queue.OtherSide.ShouldEqual(membership_queues_in_user);

        It should_set_other_side_for_the_supervisors_collection = () =>
            supervisors_in_queue.OtherSide.ShouldEqual(supervised_queues_in_user);

        It should_set_other_side_for_the_membership_queues_collection = () =>
            membership_queues_in_user.OtherSide.ShouldEqual(members_in_queue);

        It should_set_other_side_for_the_supervisored_queues_collection = () =>
            supervised_queues_in_user.OtherSide.ShouldEqual(supervisors_in_queue);

        static CollectionMapping members_in_queue;
        static CollectionMapping supervisors_in_queue;
        static CollectionMapping membership_queues_in_user;
        static CollectionMapping supervised_queues_in_user;

        private class User
        {
            public IEnumerable<Queue> GetMembershipQueues() { yield break; }
            public IEnumerable<Queue> GetSupervisedQueues() { yield break; }
        }

        private class Queue
        {
            public IEnumerable<User> GetMembers() { yield break; }
            public IEnumerable<User> GetSupervisors() { yield break; }
        }
    }

    public class when_the_bi_directional_many_to_many_visitor_is_asked_to_pair_two_many_to_manys_of_the_same_type_in_two_entities_with_names_that_have_no_likeness : BiDirectionalManyToManyPairingVisitorSpec
    {
        Establish context = () =>
        {
            fish_in_queue = collection<Queue>(x => x.Fish);
            chips_in_queue = collection<Queue>(x => x.Chips);

            bacon_in_queue = collection<User>(x => x.Bacon);
            eggs_in_queue = collection<User>(x => x.Eggs);
        };

        Because of = () =>
            ex = Catch.Exception(() => visit(fish_in_queue, chips_in_queue, bacon_in_queue, eggs_in_queue));

        It should_not_fail = () =>
            ex.ShouldBeNull();

        It should_call_the_user_defined_func = () =>
            udf_was_called.ShouldBeTrue();

        It shouldnt_set_the_other_side_of_any_of_the_relationships = () =>
        {
            fish_in_queue.OtherSide.ShouldBeNull();
            chips_in_queue.OtherSide.ShouldBeNull();
            bacon_in_queue.OtherSide.ShouldBeNull();
            eggs_in_queue.OtherSide.ShouldBeNull();
        };

        static CollectionMapping fish_in_queue;
        static CollectionMapping chips_in_queue;
        static CollectionMapping bacon_in_queue;
        static CollectionMapping eggs_in_queue;
        static Exception ex;

        private class User
        {
            public IEnumerable<Queue> Bacon { get; set; }
            public IEnumerable<Queue> Eggs { get; set; }
        }

        private class Queue
        {
            public IEnumerable<User> Fish { get; set; }
            public IEnumerable<User> Chips { get; set; }
        }
    }

    public class when_the_bi_directional_many_to_many_visitor_is_asked_to_pair_two_many_to_manys_of_the_same_type_in_two_entities_with_names_that_have_the_same_likeness : BiDirectionalManyToManyPairingVisitorSpec
    {
        Establish context = () =>
        {
            dsers_in_queue = collection<Queue>(x => x.GetDsers());
            fsers_in_queue = collection<Queue>(x => x.GetFsers());

            wueues_in_user = collection<User>(x => x.GetWueues());
            eueues_in_user = collection<User>(x => x.GetEueues());
        };

        Because of = () =>
            ex = Catch.Exception(() => visit(dsers_in_queue, fsers_in_queue, wueues_in_user, eueues_in_user));

        It should_not_fail = () =>
            ex.ShouldBeNull();

        It should_call_the_user_defined_func = () =>
            udf_was_called.ShouldBeTrue();

        It shouldnt_set_the_other_side_of_any_of_the_relationships = () =>
        {
            dsers_in_queue.OtherSide.ShouldBeNull();
            fsers_in_queue.OtherSide.ShouldBeNull();
            wueues_in_user.OtherSide.ShouldBeNull();
            eueues_in_user.OtherSide.ShouldBeNull();
        };

        static CollectionMapping dsers_in_queue;
        static CollectionMapping fsers_in_queue;
        static CollectionMapping wueues_in_user;
        static CollectionMapping eueues_in_user;
        static Exception ex;

        private class User
        {
            public IEnumerable<Queue> GetWueues() { yield break; }
            public IEnumerable<Queue> GetEueues() { yield break; }
        }

        private class Queue
        {
            public IEnumerable<User> GetDsers() { yield break; }
            public IEnumerable<User> GetFsers() { yield break; }
        }
    }

    public class when_the_bi_directional_many_to_many_visitor_is_asked_to_pair_a_many_to_many_relationship_when_one_side_has_two_possible_collections : BiDirectionalManyToManyPairingVisitorSpec
    {
        Establish context = () =>
        {
            users_in_queue = collection<Queue>(x => x.GetUsers());
            users2_in_queue = collection<Queue>(x => x.GetUsers2());

            queues_in_user = collection<User>(x => x.GetQueues());
        };

        Because of = () =>
            visit(queues_in_user, users2_in_queue, users_in_queue);

        It should_call_the_user_defined_func = () =>
            udf_was_called.ShouldBeTrue();

        It should_link_queues_in_user_to_the_most_similar_member_in_the_other_entity = () =>
            queues_in_user.OtherSide.ShouldEqual(users_in_queue);

        It should_link_users_in_queue_to_the_most_similar_member_in_the_other_entity = () =>
            users_in_queue.OtherSide.ShouldEqual(queues_in_user);

        It shouldnt_link_the_orphaned_member_with_anything = () =>
            users2_in_queue.OtherSide.ShouldBeNull();

        static CollectionMapping users_in_queue;
        static CollectionMapping users2_in_queue;
        static CollectionMapping queues_in_user;

        private class User
        {
            public IEnumerable<Queue> GetQueues() { yield break; }
        }

        private class Queue
        {
            public IEnumerable<User> GetUsers() { yield break; }
            public IEnumerable<User> GetUsers2() { yield break; }
        }
    }

    public class when_the_bi_directional_many_to_many_visitor_is_asked_to_pair_two_collections_that_are_exposed_through_methods : BiDirectionalManyToManyPairingVisitorSpec
    {
        Establish context = () =>
        {
            users_in_queue = collection<Queue>(x => x.GetUsers());
            queues_in_user = collection<User>(x => x.GetQueues());
        };

        Because of = () =>
            visit(users_in_queue, queues_in_user);

        It should_call_the_user_defined_func = () =>
            udf_was_called.ShouldBeTrue();

        It should_set_other_side_for_the_users_collection = () =>
            users_in_queue.OtherSide.ShouldEqual(queues_in_user);

        It should_set_other_side_for_the_queues_collection = () =>
            queues_in_user.OtherSide.ShouldEqual(users_in_queue);

        static CollectionMapping users_in_queue;
        static CollectionMapping queues_in_user;

        private class User
        {
            public IEnumerable<Queue> GetQueues() { yield break; }
        }

        private class Queue
        {
            public IEnumerable<User> GetUsers() { yield break; }
        }
    }

    #region spec base

    public abstract class BiDirectionalManyToManyPairingVisitorSpec
    {
        Establish context = () =>
            visitor = new RelationshipPairingVisitor((c, o, w) => udf_was_called = true);

        static RelationshipPairingVisitor visitor;
        protected static bool udf_was_called;

        protected static CollectionMapping collection<T>(Expression<Func<T, object>> expression)
        {
            var member = expression.ToMember();

            var bag = CollectionMapping.Bag();
            
            bag.ContainingEntityType = typeof(T);
            bag.Member = member;
            bag.Relationship = new ManyToManyMapping();
            bag.ChildType = member.PropertyType.GetGenericArguments()[0];
            
            return bag;
        }

        protected static void visit(params CollectionMapping[] mappings)
        {
            mappings.Each(visitor.Visit);
            visitor.Visit(new HibernateMapping[0]); // simulate end of visit
        }
    }

    #endregion
}
