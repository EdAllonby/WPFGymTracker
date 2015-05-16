﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using GymTracker.Models;
using GymTracker.ViewModels.Annotations;

namespace GymTracker.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private WorkoutItemViewModel selectedWorkout;
        private string searchWorkouts;

        public MainViewModel()
        {
            using (var context = new GymContext())
            {
                List<Workout> workouts = context.Workouts.ToList();

                Workouts = new ObservableCollection<WorkoutItemViewModel>();
                Exercises = new ObservableCollection<StrengthSetItemViewModel>();

                foreach (Workout workout in workouts)
                {
                    Workouts.Add(new WorkoutItemViewModel(workout));
                }
            }
        }

        public static string Title => "Gym Tracker";

        public ObservableCollection<WorkoutItemViewModel> Workouts { get; set; }

        public ObservableCollection<StrengthSetItemViewModel> Exercises { get; set; } 

        public WorkoutItemViewModel SelectedWorkout
        {
            get { return selectedWorkout; }
            set
            {
                Exercises.Clear();
                selectedWorkout = value;

                foreach (Exercise exercise in SelectedWorkout.Exercises)
                {
                    var strengthSetExercise = exercise as StrengthSet;

                    if (strengthSetExercise != null)
                    {
                        Exercises.Add(new StrengthSetItemViewModel((StrengthSet) exercise));
                    }
                }

                OnPropertyChanged();
            }
        }


        public string SearchWorkouts
        {
            get { return searchWorkouts; }
            set
            {
                searchWorkouts = value;

                OnPropertyChanged();
            }
        }


        public StrengthSetItemViewModel SelectedStrengthSet { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}