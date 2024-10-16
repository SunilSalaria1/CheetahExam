using Microsoft.EntityFrameworkCore;

namespace CheetahExam.WebUI.Shared.Enums;

public static class Enum
{
    [Comment("We have this static class for all the question types we have")]
    public static class QuestionTypes
    {
        [Comment(nameof(CheckBox))]
        public const int CheckBox = 1;

        [Comment(nameof(MultipleChoice))]
        public const int MultipleChoice = 2;

        [Comment(nameof(TrueFalse))]
        public const int TrueFalse = 3;

        [Comment(nameof(FillInTheBlank))]
        public const int FillInTheBlank = 4;

        [Comment(nameof(DropDown))]
        public const int DropDown = 5;

        [Comment(nameof(Essay))]
        public const int Essay = 6;

        [Comment(nameof(OrderList))]
        public const int OrderList = 7;

        [Comment(nameof(Note))]
        public const int Note = 8;

        [Comment(nameof(AudioVideo))]
        public const int AudioVideo = 9;

        [Comment(nameof(Comprehension))]
        public const int Comprehension = 10;

        [Comment(nameof(Document))]
        public const int Document = 11;

        [Comment(nameof(RecordAudioVideo))]
        public const int RecordAudioVideo = 12;

        [Comment(nameof(Matching))]
        public const int Matching = 13;

        [Comment(nameof(DragDropWithText))]
        public const int DragDropWithText = 14;

        [Comment(nameof(DragDropWithMatching))]
        public const int DragDropWithMatching = 15;
        
        [Comment(nameof(Hotspot))]
        public const int Hotspot = 16;
        
        [Comment(nameof(ImportQuestions))]
        public const int ImportQuestions = 17;

        [Comment(nameof(RadioButtons))]
        public const int RadioButtons = 18;
    }
}
