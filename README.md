```mermaid
gantt
    title A Gantt Diagram
    dateFormat  YYYY-MM-DD
    excludes weekdays 2024-01-10

    section Section A
    Task 1      :a1, 2024-01-01, 10d
    Task 2      :after a1, 20d

    section Section B
    Task 3      :2024-01-02, 12d
    Task 4      :24d
