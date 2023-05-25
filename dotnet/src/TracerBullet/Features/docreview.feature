Feature: DocReview

    Background:
        Given DocReviewSettings:
          | isCommentingAllowed | isSubcommentingAllowed | AreEmojiOnCommentsAllowed | isClosedForcomments | isLoginRequired |
          | true                | true                   | true                      | false               | false           |
          | true                | true                   | true                      | false               | false           |

        Given Users:
          | userId | firstname | lastname  | email                          | password |
          | 1      | sander    | Verheyen  | sander.verheyen@student.kdg.be | sander   |
          | 2      | niels     | Van Steen | niels.vansteen@student.kdg.be  | niels    |

        Given DocReviews:
          | docreview | docreviewname | commentAmount | description | bannerImagePath | docReviewText | writtenbyId |
          | 1         | doopcharter   | 2             | doopcharter | /image.jpg      | some text     | 1           |
          | 2         | gemeentedebat | 5             | debat       | /image.png      | some text     | 1           |

        Given Comments:
          | comment | commentText                    | user | docreview | subcomment |
          | 1       | Dit is een comment             | 1    | 2         | null       |
          | 2       | Dit is nog een comment         | 2    | 2         | null       |
          | 3       | Dit is weer een comment        | 1    | 1         | null       |
          | 4       | Dit document trekt op niks.... | 2    | 1         | null       |

    Scenario: Showing all comments from DocReview
        Given There is a docReview 1
        When DocReview 1 loads
        Then the comments should contain the following comments:
          | comment | commentText                    | user | docreview | subcomment |
          | 3       | Dit is weer een comment        | 1    | 1         | null       |
          | 4       | Dit document trekt op niks.... | 2    | 1         | null       |

    Scenario: Adding a comment to a DocReview
        Given There is a docReview 2
        And User 1 is logged in
        When User 1 writes a comment 5 "Dit is een comment op docreview 2" on characters 5 - 35
        And Clicks the publish button
        Then DocReview 2 now has 3 comments
        And User 1 has comment 5
        And DocReview 2 page now shows comment 5

    Scenario: Adding a comment on a comment
        Given User 1 has a comment 3 on DocReview 1
        And User 2 is logged in
        When User 2 writes a comment 6 "Dit is een comment op comment 1" on comment 3
        And User 2 clicks the publish button
        Then comment 3 should have a subcomment: comment 6