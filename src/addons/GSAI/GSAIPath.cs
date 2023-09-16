using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

namespace GodotSteeringAI
{
    /// <summary>
    /// Represents a path made up of Vector3 waypoints, split into segments path follow behaviors can use.
    /// </summary>
    public partial class GSAIPath : RefCounted
    {
        /// <summary>
        /// If `false`, the path loops.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Total length of the path.
        /// </summary>
        public float Length { get; set; }

        private List<GSAISegment> _segments;
        private Vector3 _nearest_point_on_segment;
        private Vector3 _nearest_point_on_path;

        public GSAIPath(List<Vector3> waypoints, bool is_open = false)
        {
            IsOpen = is_open;
            CreatePath(waypoints);
            _nearest_point_on_segment = waypoints[0];
            _nearest_point_on_path = waypoints[0];
        }

        /// <summary>
        /// Creates a path from a list of waypoints.
        /// </summary>
        /// <param name="waypoints"></param>
        public void CreatePath(List<Vector3> waypoints)
        {
            if (waypoints is null || waypoints.Count < 2)
            {
                GD.PrintErr("Waypoints cannot be null and must contain at least two (2) waypoints.");
                return;
            }

            _segments = new List<GSAISegment>();
            Length = 0;

            var current = waypoints[0];
            Vector3 previous;
            for (int i = 1; i < waypoints.Count; i++)
            {
                previous = current;
                if (i < waypoints.Count)
                    current = waypoints[i];
                else if (IsOpen)
                    break;
                else
                    current = waypoints[0];
                var segment = new GSAISegment(previous, current);
                Length += segment.length;
                segment.cumulative_length = Length;
                _segments.Add(segment);
            }
        }

        /// <summary>
        /// Returns the distance from `agent_current_position` to the next waypoint.
        /// </summary>
        /// <param name="agent_current_position"></param>
        /// <returns></returns>
        public float CalculateDistance(Vector3 agent_current_position)
        {
            if (_segments.Count == 0)
                return 0;
            var smallest_distance_squared = float.PositiveInfinity;
            GSAISegment nearest_segment = null;
            foreach (var segment in _segments)
            {
                var distance_squared = _CalculatePointSegmentDistanceSquared(segment.begin, segment.end, agent_current_position);
                if (distance_squared < smallest_distance_squared)
                {
                    _nearest_point_on_path = _nearest_point_on_segment;
                    smallest_distance_squared = distance_squared;
                    nearest_segment = segment;
                }
            }
            var length_on_path = nearest_segment.cumulative_length - _nearest_point_on_path.DistanceTo(nearest_segment.end);
            return length_on_path;
        }

        /// <summary>
        /// Calculates a target position from the path's starting point based on the `target_distance`.
        /// </summary>
        /// <param name="target_distance"></param>
        /// <returns></returns>
        public Vector3 CalculateTargetPosition(float target_distance)
        {
            if (IsOpen)
                target_distance = Mathf.Clamp(target_distance, 0, Length);
            else
            {
                if (target_distance < 0)
                    target_distance = Length + target_distance % Length;
                else if (target_distance > Length)
                    target_distance = target_distance % Length;
            }

            GSAISegment desired_segment = null;
            foreach (var seg in _segments)
            {
                if (seg.cumulative_length >= target_distance)
                {
                    desired_segment = seg;
                    break;
                }
            }

            if (desired_segment is null)
                desired_segment = _segments[_segments.Count - 1];

            var distance = desired_segment.cumulative_length - target_distance;

            return (desired_segment.begin - desired_segment.end)
                * (distance / desired_segment.length)
                + desired_segment.end;
        }

        /// <summary>
        /// Returns the position of the first point on the path.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetStartPoint()
        {
            return _segments[0].begin;
        }

        /// <summary>
        /// Returns the position of the last point on the path.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetEndPoint()
        {
            return _segments[_segments.Count - 1].end;
        }

        private float _CalculatePointSegmentDistanceSquared(Vector3 start, Vector3 end, Vector3 position)
        {
            _nearest_point_on_segment = start;
            var start_end = end - start;
            var start_end_length_squared = start_end.LengthSquared();
            if (start_end_length_squared != 0)
            {
                var t = (position - start).Dot(start_end) / start_end_length_squared;
                _nearest_point_on_segment += start_end * Mathf.Clamp(t, 0, 1);
            }
            return _nearest_point_on_segment.DistanceSquaredTo(position);
        }

        class GSAISegment
        {
            public Vector3 begin;
            public Vector3 end;
            public float length;
            public float cumulative_length;

            public GSAISegment(Vector3 _begin, Vector3 _end)
            {
                begin = _begin;
                end = _end;
                length = begin.DistanceTo(end);
            }
        }
    }
}
